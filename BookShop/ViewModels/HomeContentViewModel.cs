using BookShop.Infrastructure.Commands;
using BookShop.ViewModels.Base;
using System.Linq;
using DataAccess;
using System.Collections.Generic;

namespace BookShop.ViewModels
{
    public class HomeContentViewModel : ViewModel
    {
        private HomeViewModel _main;
        private Book _selectedItem;
        private LambdaCommand _addToCart;
        private LambdaCommand _showBookPage;

        public HomeContentViewModel(HomeViewModel homeViewModel)
        {
            _main = homeViewModel;
            AllBooks = _main.db.Books.Items.ToList();
        }

        public List<Book> AllBooks { get; set; }
        public Book SelectedItem
        {
            get { return _selectedItem; }
            set => Set(ref _selectedItem, value);
        }
        public LambdaCommand ShowBookPage
        {
            get
            {
                return _showBookPage ?? (_showBookPage = new LambdaCommand((o) =>
                {
                    if (SelectedItem != null)
                    {
                        //переход на страницу книги
                    }
                }));
            }
        }
        public LambdaCommand AddToCart
        {
            get
            {
                return _addToCart ?? (_addToCart = new LambdaCommand((o) =>
                {
                    var movie = o as Book;

                    if (movie != null)
                    {
                        //Добавление в корзину и попап
                    }
                }));
            }
        }
    }
}
