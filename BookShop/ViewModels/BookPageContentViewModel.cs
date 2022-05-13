using BookShop.Infrastructure.Commands;
using BookShop.ViewModels.Base;
using DataAccess;

namespace BookShop.ViewModels
{
    class BookPageContentViewModel : ViewModel
    {
        private HomeViewModel _main;
        private readonly Book _bookContext;
        private LambdaCommand _changeSelectedImage;
        private LambdaCommand _addToBasket;
        private LambdaCommand _showCategoryPage;
        private Photo _selectedImage;

        public BookPageContentViewModel(HomeViewModel _main, Book book)
        {
            this._main = _main;
            _bookContext = book;
            SelectedImage = _bookContext.Photos[0];
        }

        public LambdaCommand ChangeSelectedImage
        {
            get
            {
                return _changeSelectedImage ?? (_changeSelectedImage = new LambdaCommand((o) =>
                {
                    var img = o as Photo;
                    if (img != null)
                    {
                        SelectedImage = img;
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
        public LambdaCommand ShowCategoryPage
        {
            get
            {
                return _showCategoryPage ?? (_showCategoryPage = new LambdaCommand((o) =>
                {
                    var category = o as Category;
                    if (category != null)
                    {
                        _main.ShowCategoryPage.Execute(category);
                    }
                }));
            }
        }
        public Photo SelectedImage
        {
            get => _selectedImage;
            set => Set(ref _selectedImage, value);
        }
        public Book BookContext
        {
            get => _bookContext;
        }
    }
}
