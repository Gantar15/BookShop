using BookShop.Infrastructure.Commands;
using BookShop.Models;
using BookShop.Services;
using BookShop.ViewModels.Base;
using BookShop.ViewModels.Common;
using DataAccess;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BookShop.ViewModels
{
    public class BasketPageContentViewModel : ViewModel
    {
        private HomeViewModel _main;
        private decimal _basketPrice;
        private int _basketCount;
        private LambdaCommand _showBookPage;
        private LambdaCommand _removeCommand;
        private LambdaCommand _incrementCommand;
        private LambdaCommand _decrementCommand;
        private LambdaCommand _showOrderPage;
        private readonly MessageBoxService _messageBoxService;

        public BasketPageContentViewModel(HomeViewModel _main)
        {
            _messageBoxService = new MessageBoxService();
            this._main = _main;

            int userId = LoggedinUser.Id;
            CurrentBasket = _main.db.Baskets.GetFirstOrDefault(basket => basket.UserId == userId);

            var booksItems = new List<Book>();
            CurrentBasket.Products.ForEach(p => booksItems.Add(p.Book));
            BasketItems = new ObservableCollection<BasketProductInfo>();
            foreach(var bookitem in booksItems)
            {
                var basketProductInfo = new BasketProductInfo();
                var basketProduct = _main.db.BasketProducts.GetFirstOrDefault(bp => bp.ProductId == bookitem.Product.Id && bp.BasketId == CurrentBasket.Id);
                basketProductInfo.Book = bookitem;
                if (basketProduct != null)
                {
                    basketProductInfo.Count = basketProduct.Count;
                    basketProductInfo.TotalСost = basketProduct.Count * bookitem.Product.Price;
                }
                BasketItems.Add(basketProductInfo);
                UpdateBasket();
            }
        }

        public void UpdateBasket()
        {
            decimal newPrice = 0;
            foreach (var bpItem in BasketItems)
            {
                var bpProduct = _main.db.Products.GetFirstOrDefault(p => p.BookId == bpItem.Book.Id);
                newPrice += bpItem.Book.Product.Price * CurrentBasket.BasketProducts.First(bp => bp.ProductId == bpProduct.Id).Count;
            }
            BasketPrice = newPrice;

            int newCount = 0;
            CurrentBasket.BasketProducts.ForEach(bp => newCount += bp.Count);
            BasketCount = newCount;

            _main.UpdateBasket();
        }
        public void ClearBasket()
        {
            foreach (var basketProduct in CurrentBasket.BasketProducts.ToList())
            {
                _main.db.BasketProducts.Remove(basketProduct.Id);
            }
            _main.UpdateBasket();
        }

        public LambdaCommand ShowBookPage
        {
            get
            {
                return _showBookPage ?? (_showBookPage = new LambdaCommand((o) =>
                {
                    var book = o as Book;
                    if (book != null)
                    {
                        _main.ShowBookPage.Execute(book);
                    }
                }));
            }
        }
        public LambdaCommand ShowOrderPage
        {
            get
            {
                return _showOrderPage ?? (_showOrderPage = new LambdaCommand((o) =>
                {
                    if (BasketCount == 0) {
                        _messageBoxService.ShowMessageBox("Корзина", "Корзина пуста", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                        return;
                    }
                    _main.ShowingViewModel = new OrderPageContentViewModel(_main, this);
                }));
            }
        }
        public LambdaCommand RemoveCommand
        {
            get
            {
                return _removeCommand ?? (_removeCommand = new LambdaCommand((o) =>
                {
                    var book = o as Book;
                    if (book != null)
                    {
                        var product = _main.db.Products.Get(book.Id);
                        CurrentBasket.Products.Remove(product);
                        _main.db.Baskets.Update(CurrentBasket);
                        BasketItems.Remove(BasketItems.FirstOrDefault(item => item.Book.Id == book.Id));
                        UpdateBasket();
                    }
                }));
            }
        }
        public LambdaCommand IncrementCommand
        {
            get
            {
                return _incrementCommand ?? (_incrementCommand = new LambdaCommand(o =>
                {
                    var book = o as Book;
                    if (book != null)
                    {
                        var basketProduct = CurrentBasket.BasketProducts.FirstOrDefault(bp => bp.ProductId == book.Product.Id);
                        basketProduct.Count++;
                        _main.db.BasketProducts.Update(basketProduct);
                        BasketItems.First(item => item.Book.Id == book.Id).Count++;
                        UpdateBasket();
                    }
                }));
            }
        }
        public LambdaCommand DecrementCommand
        {
            get
            {
                return _decrementCommand ?? (_decrementCommand = new LambdaCommand(o =>
                {
                    var book = o as Book;
                    if (book != null)
                    {
                        var basketProductInfo = BasketItems.First(item => item.Book.Id == book.Id);

                        var basketProduct = CurrentBasket.BasketProducts.FirstOrDefault(bp => bp.ProductId == book.Product.Id);
                        basketProduct.Count--;
                        _main.db.BasketProducts.Update(basketProduct);
                        BasketItems.First(item => item.Book.Id == book.Id).Count--;
                        UpdateBasket();

                        if (basketProductInfo.Count == 0)
                            RemoveCommand.Execute(book);
                    }
                }));
            }
        }
        public Basket CurrentBasket { get; }
        public ObservableCollection<BasketProductInfo> BasketItems { get; set; }
        public decimal BasketPrice
        {
            get => _basketPrice;
            set => Set(ref _basketPrice, value);
        }
        public int BasketCount
        {
            get => _basketCount;
            set => Set(ref _basketCount, value);
        }
    }
}
