using BookShop.Infrastructure.Commands;
using BookShop.Services;
using BookShop.ViewModels.Base;
using System.Windows;
using DataAccess;
using BookShop.Models;
using System.Collections.Generic;
using BookShop.ViewModels.Common;
using System;
using BookShop.Infrastructure;

namespace BookShop.ViewModels
{
    public class OrderPageContentViewModel : ViewModel
    {
        private HomeViewModel _main;
        private BasketPageContentViewModel _basket;
        private LambdaCommand _orderCommand;
        private ConfigurationFactory _configurationFactory;
        private readonly MessageBoxService _messageBoxService;
        private readonly EmailService _emailService;

        public OrderPageContentViewModel(HomeViewModel _main, BasketPageContentViewModel _basket)
        {
            _messageBoxService = new MessageBoxService();
            _emailService = new EmailService();
            _configurationFactory = new ConfigurationFactory();
            this._basket = _basket;
            this._main = _main;
        }

        private Order CreateOrder()
        {
            var orderProducts = new List<OrderProduct>();
            var newOrder = new Order
            {
                Address = OrderFormModel.Address,
                Fio = OrderFormModel.Fio,
                Phone = OrderFormModel.Phone,
                UserId = LoggedinUser.Id
            };
            _main.db.Orders.Add(newOrder);
            foreach (var basketProduct in _basket.CurrentBasket.BasketProducts)
            {
                var orderProduct = new OrderProduct
                {
                    Count = basketProduct.Count,
                    Product = basketProduct.Product,
                    Order = newOrder
                };
                _main.db.OrderProducts.Add(orderProduct);
                orderProducts.Add(orderProduct);
            }

            return newOrder;
        }

        private string GetOrdersHtmlTable(Order currentOrder)
        {
            string productsHtmlTr = "";
            var orderProducts = _main.db.OrderProducts.Get(op => op.OrderId == currentOrder.Id);
            foreach (var orderProduct in orderProducts)
            {
                var book = _main.db.Books.Get((int)orderProduct.Product.BookId);
                productsHtmlTr += "<tr>" +
                                $"<td>{book.Title}</td>" +
                                $"<td>{String.Format("{0:C2}", orderProduct.Product.Price)}</td>" +
                                $"<td>{orderProduct.Count}</td>" +
                                "</tr>";
            }

            return productsHtmlTr;
        }

        public LambdaCommand OrderCommand
        {
            get
            {
                return _orderCommand ?? (_orderCommand = new LambdaCommand(async o =>
                {
                    var configuration = _configurationFactory.GetConfiguration();
                    string AdminMail = configuration["mailInfo:adminMail"];

                    var currentOrder = CreateOrder();
                    _messageBoxService.ShowMessageBox("Заказ", $"Ваш заказ успешно оформлен :3", MessageBoxButton.OK, MessageBoxImage.Information);

                    string messageUserBody = $"<article>" +
                    $"<h2>Уважаемый {currentOrder.Fio}, ваш заказ: </h2>" +
                    "<table margin=\"10,0,0,0\">" +
                    "<thead><tr>" +
                        "<th>название</td>" +
                        "<th>цена</td>" +
                        "<th>количество</th>" +
                    "</tr></thead>" +
                    "<tbody>" +
                        GetOrdersHtmlTable(currentOrder) +
                    "</tbody>" +
                    "</table>" +
                    "</article>";

                    string messageAdminBody = $"<article>" +
                    $"<h2>Заказ на имя {currentOrder.Fio}</h2>" +
                    $"<p>Адрес получателя {currentOrder.Address}</p>" +
                    $"<p>Телефон получателя получателя {currentOrder.Phone}</p>" +
                    "<table margin=\"10,0,0,0\">" +
                    "<thead><tr>" +
                        "<th>название</td>" +
                        "<th>цена</td>" +
                        "<th>количество</th>" +
                    "</tr></thead>" +
                    "<tbody>" +
                        GetOrdersHtmlTable(currentOrder) +
                    "</tbody>" +
                    "</table>" +
                    "</article>";
                    await _emailService.SendMail(LoggedinUser.Email, $"{currentOrder.Fio}, ваш заказ успешно оформлен :3", messageUserBody, true);
                    _emailService.SendMail(AdminMail, $"Оформлен заказ N{currentOrder.Id}", messageAdminBody, true);

                    _basket.ClearBasket();
                    _main.ChangeCommand.Execute("home");
                },
                (o) => !OrderFormModel.HasErrors));
            }
        }
        public OrderForm OrderFormModel { get; } = new OrderForm();
        public decimal BasketPrice
        {
            get => _basket.BasketPrice;
        }
        public int BasketCount
        {
            get => _basket.BasketCount;
        }
    }
}
