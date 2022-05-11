using BookShop.Infrastructure.Commands;
using BookShop.Services;
using BookShop.ViewModels.Base;
using System.Windows;
using DataAccess;
using BookShop.Models;
using System.Collections.Generic;

namespace BookShop.ViewModels
{
    public class OrderPageContentViewModel : ViewModel
    {
        private HomeViewModel _main;
        private BasketPageContentViewModel _basket;
        private LambdaCommand _orderCommand;
        private readonly MessageBoxService _messageBoxService;

        public OrderPageContentViewModel(HomeViewModel _main, BasketPageContentViewModel _basket)
        {
            _messageBoxService = new MessageBoxService();
            this._basket = _basket;
            this._main = _main;
        }

        private void CreateOrder()
        {
            var orderProducts = new List<OrderProduct>();
            foreach (var basketProduct in _basket.CurrentBasket.BasketProducts)
            {
                var orderProduct = new OrderProduct
                {
                    Count = basketProduct.Count,
                    Product = basketProduct.Product
                };
                orderProducts.Add(orderProduct);
            }
            var newOrder = new Order
            {
                Address = OrderFormModel.Address,
                Fio = OrderFormModel.Fio,
                Phone = OrderFormModel.Phone
            };
            _main.db.Orders.Add(newOrder);
            newOrder.OrderProducts.AddRange(orderProducts);
        }

        public LambdaCommand OrderCommand
        {
            get
            {
                return _orderCommand ?? (_orderCommand = new LambdaCommand(o =>
                {
                    CreateOrder();
                    _messageBoxService.ShowMessageBox("Заказ", "Ваш заказ успешно оформлен", MessageBoxButton.OK, MessageBoxImage.Information);
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
