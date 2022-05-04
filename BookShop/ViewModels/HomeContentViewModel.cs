using BookShop.Infrastructure.Commands;
using BookShop.Services;
using BookShop.ViewModels.Base;
using BookShop.ViewModels.Common;
using DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace BookShop.ViewModels
{
    public class HomeContentViewModel : ViewModel
    {
        private HomeViewModel _main;
        private LambdaCommand _addToBasket;
        private LambdaCommand _showBookPage;
        private readonly MessageBoxService _messageBoxService;
        private List<Book> _allBooks;

        public HomeContentViewModel(HomeViewModel homeViewModel)
        {
            _main = homeViewModel;
            _messageBoxService = new MessageBoxService();
            AllBooks = _main.AllBooks;
        }

        public List<Book> AllBooks
        {
            get => _allBooks;
            set => Set(ref _allBooks, value);
        }
        public LambdaCommand ShowBookPage
        {
            get
            {
                return _showBookPage ?? (_showBookPage = new LambdaCommand((o) =>
                {
                    var movie = o as Book;
                    if (movie != null)
                    {
                        //переход на страницу книги
                    }
                }));
            }
        }
        public LambdaCommand AddToBasket
        {
            get
            {
                return _addToBasket ?? (_addToBasket = new LambdaCommand((o) =>
                {
                    var movie = o as Book;

                    if (movie != null)
                    {
                        if (!movie.InStock)
                        {
                            _messageBoxService.ShowMessageBox(
                            movie.Title,
                            "Товара нет в наличии",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                            return;
                        }

                        var loggedinUserBasket = _main.db.Baskets.Get(b => b.UserId == LoggedinUser.Id)[0];
                        var existsBasketProduct = loggedinUserBasket.BasketProducts.FirstOrDefault(bp => bp.ProductId == movie.ProductId);
                        if (loggedinUserBasket.BasketProducts.Count == 0 || existsBasketProduct == null) {
                            var basketProduct = new BasketProduct
                            {
                                Basket = loggedinUserBasket,
                                Product = movie.Product,
                                Count = 1
                            };
                            _main.db.BasketProducts.Add(basketProduct);
                        }
                        else if(existsBasketProduct != null)
                        {
                            existsBasketProduct.Count++;
                            _main.db.BasketProducts.Update(existsBasketProduct);
                        }
                        _main.UpdateBasket();
                    }
                }));
            }
        }
    }
}
