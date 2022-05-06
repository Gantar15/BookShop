using BookShop.Infrastructure.Commands;
using BookShop.ViewModels.Base;
using BookShop.ViewModels.Common;
using DataAccess;
using System.Collections.Generic;

namespace BookShop.ViewModels
{
    public class BasketPageContentViewModel : ViewModel
    {
        private HomeViewModel _main;
        private List<Book> _basketItems;
        private decimal _basketPrice;
        private int _basketCount;
        private LambdaCommand _showBookPage;
        private LambdaCommand _removeCommand;

        public BasketPageContentViewModel(HomeViewModel _main)
        {
            this._main = _main;

            int userId = LoggedinUser.Id;
            CurrentBasket = _main.db.Baskets.GetFirstOrDefault(basket => basket.UserId == userId);
            var items = new List<Book>();
            CurrentBasket.Products.ForEach(p => items.Add(p.Book));
            BasketItems = items;
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
                        _main.db.Save();
                    }
                }));
            }
        }
        private Basket CurrentBasket { get; }
        public List<Book> BasketItems
        {
            get => _basketItems;
            set 
            {
                Set(ref _basketItems, value);

                decimal newPrice = 0;
                _basketItems.ForEach(b => newPrice += b.Product.Price);
                BasketPrice = newPrice;

                int newCount = 0;
                CurrentBasket.BasketProducts.ForEach(bp => newCount += bp.Count);
                BasketCount = newCount;
            }
        }
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
