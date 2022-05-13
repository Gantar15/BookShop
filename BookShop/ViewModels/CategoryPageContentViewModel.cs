using BookShop.Infrastructure.Commands;
using BookShop.ViewModels.Base;
using DataAccess;
using System.Collections.Generic;

namespace BookShop.ViewModels
{
    class CategoryPageContentViewModel : ViewModel
    {
        private HomeViewModel _main;
        private LambdaCommand _addToBasket;
        private LambdaCommand _showBookPage;
        private List<Book> _allBooks;
        private Category _currentCategory;

        public CategoryPageContentViewModel(HomeViewModel homeViewModel, Category category)
        {
            _main = homeViewModel;
            CurrentCategory = category;
            AllBooks = _main.db.Books.Get(book => book.CategoryId == CurrentCategory.Id);
        }

        public Category CurrentCategory 
        { 
            get => _currentCategory;
            set => Set(ref _currentCategory, value);
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
                    var book = o as Book;
                    if (book != null)
                    {
                        _main.ShowBookPage.Execute(book);
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
                    var book = o as Book;

                    if (book != null)
                    {
                        _main.AddToBasket.Execute(book);
                    }
                }));
            }
        }
    }
}
