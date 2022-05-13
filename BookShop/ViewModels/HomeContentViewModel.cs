using BookShop.Infrastructure.Commands;
using BookShop.ViewModels.Base;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace BookShop.ViewModels
{
    public class HomeContentViewModel : ViewModel
    {
        private HomeViewModel _main;
        private LambdaCommand _addToBasket;
        private LambdaCommand _showBookPage;
        private Book _showingSliderBook;
        private int _showingIndex = 0;
        private int _showingEndIndex = 6;
        private bool _changed = false;
        private List<Book> _allBooks;
        private List<Book> _sliderBooks;
        private DispatcherTimer _timer = new DispatcherTimer();

        public HomeContentViewModel(HomeViewModel homeViewModel)
        {
            _main = homeViewModel;
            AllBooks = _main.AllBooks;

            if (AllBooks.Count > 0)
            {
                if (AllBooks.Count-1 < _showingEndIndex)
                    _showingEndIndex = AllBooks.Count - 1;
                SliderBooks = AllBooks.GetRange(0, _showingEndIndex + 1);

                ShowingSliderBook = AllBooks[_showingIndex];
                _timer.Interval = TimeSpan.FromSeconds(8);
                _timer.Tick += TimerTick;
                _timer.Start();
            }
            _main.ChangeBooksList = (bookList) =>
            {
                AllBooks = bookList;
            };
        }

        private async void TimerTick(object? sender, EventArgs e)
        {
            _showingIndex++;
            if (_showingIndex > _showingEndIndex)
            {
                _showingIndex = 0;
            }
            Changed = true;
            await Task.Delay(700);
            ShowingSliderBook = SliderBooks[_showingIndex];
            Changed = false;
        }

        public bool Changed
        {
            get => _changed;
            set => Set(ref _changed, value);
        }
        public Book ShowingSliderBook
        {
            get => _showingSliderBook;
            set => Set(ref _showingSliderBook, value);
        }
        public List<Book> AllBooks
        {
            get => _allBooks;
            set => Set(ref _allBooks, value);
        }
        public List<Book> SliderBooks
        {
            get => _sliderBooks;
            set => Set(ref _sliderBooks, value);
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
