using BookShop.Infrastructure.Commands;
using BookShop.Models;
using BookShop.Services;
using BookShop.ViewModels.Base;
using BookShop.ViewModels.Common;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace BookShop.ViewModels
{
    public class HomeViewModel : ViewModel
    {
        private ViewModel _ShowingViewModel;
        private readonly UnitOfWork _unitOfWork;
        private string _searchText;
        private readonly MessageBoxService _messageBoxService;
        private List<Book> _allBooks;
        private LambdaCommand _searchCommand;
        private LambdaCommand _addToBasket;

        public HomeViewModel(UnitOfWork unitOfWork = null)
        {
            _messageBoxService = new MessageBoxService();

            if (unitOfWork == null)
                _unitOfWork = new UnitOfWork();
            else
                _unitOfWork = unitOfWork;

            ResetAllBooks();
            ShowingViewModel = new HomeContentViewModel(this);
            ChangeCommand = new LambdaCommand((name) =>
            {
                switch (name.ToString())
                {
                    case "home":
                        ResetAllBooks();
                        if(ShowingViewModel.GetType().Name != "HomeContentViewModel")
                            ShowingViewModel = new HomeContentViewModel(this);
                        break;
                    case "contacts":
                        if (ShowingViewModel.GetType().Name != "ContactsVeiwModel")
                            ShowingViewModel = new ContactsVeiwModel();
                        break;
                    case "deliveryInfo":
                        if (ShowingViewModel.GetType().Name != "DeliveryInfoVeiwModel")
                            ShowingViewModel = new DeliveryInfoVeiwModel();
                        break;
                }
            });
            UpdateBasket();
        }

        public void ResetAllBooks()
        {
            AllBooks = db.Books.Items.ToList();
            ChangeBooksList?.Invoke(AllBooks);
        }
        public void UpdateBasket()
        {
            BasketModelContext.Count = GetActualBasketCount();
            BasketModelContext.Price = GetActualBasketPrice();
        }
        public int GetActualBasketCount() {
            var loggedinUserBasket = db.Baskets.Get(b => b.UserId == LoggedinUser.Id)[0];
            var basketProducts = db.BasketProducts.Get(bp => bp.BasketId == loggedinUserBasket.Id);
            if (basketProducts == null)
                return 0;
            int allBpCount = 0;
            basketProducts.ForEach(bp => allBpCount += bp.Count);
            return allBpCount;
        }
        public decimal GetActualBasketPrice()
        {
            var loggedinUserBasket = db.Baskets.Get(b => b.UserId == LoggedinUser.Id)[0];
            var basketProducts = db.BasketProducts.Get(bp => bp.BasketId == loggedinUserBasket.Id);
            if (basketProducts == null)
                return 0;
            decimal allBpPrice = 0;
            basketProducts.ForEach(bp => allBpPrice += bp.Count * bp.Product.Price);
            return allBpPrice;
        }

        public UnitOfWork db { get => _unitOfWork; }
        public Action<List<Book>> ChangeBooksList { get; set; }
        public LambdaCommand ChangeCommand { get; set; }
        public LambdaCommand SearchCommand
        {
            get {
                return _searchCommand ?? (_searchCommand = new LambdaCommand((o) =>
                {
                    ChangeCommand.Execute("home");

                    List<Book> searchResults = new List<Book>();
                    var titleSearch = db.Books.Get(b => Regex.IsMatch(b.Title, $"^.*{SearchText}.*$", RegexOptions.IgnoreCase));
                    var authorsSearch = db.Books.Get(b => b.Authors.Any(a => Regex.IsMatch($"{a.Name} {a.Surname}", $"^.*{SearchText}.*$", RegexOptions.IgnoreCase)));
                    var categorySearch = db.Books.Get(b => Regex.IsMatch(b.Category.Title, $"^.*{SearchText}.*$", RegexOptions.IgnoreCase));

                    if(titleSearch != null)
                        searchResults.AddRange(titleSearch);
                    if (authorsSearch != null)
                        searchResults.AddRange(authorsSearch);
                    if (categorySearch != null)
                        searchResults.AddRange(categorySearch);

                    searchResults = searchResults.Select(b => b).Distinct().ToList();

                    AllBooks = searchResults;
                    ChangeBooksList?.Invoke(AllBooks);
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
                        if (!book.InStock)
                        {
                            _messageBoxService.ShowMessageBox(
                            book.Title,
                            "Товара нет в наличии",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                            return;
                        }

                        var loggedinUserBasket = db.Baskets.Get(b => b.UserId == LoggedinUser.Id)[0];
                        var existsBasketProduct = loggedinUserBasket.BasketProducts.FirstOrDefault(bp => bp.ProductId == book.ProductId);
                        if (loggedinUserBasket.BasketProducts.Count == 0 || existsBasketProduct == null)
                        {
                            var basketProduct = new BasketProduct
                            {
                                Basket = loggedinUserBasket,
                                Product = book.Product,
                                Count = 1
                            };
                            db.BasketProducts.Add(basketProduct);
                        }
                        else if (existsBasketProduct != null)
                        {
                            existsBasketProduct.Count++;
                            db.BasketProducts.Update(existsBasketProduct);
                        }
                        UpdateBasket();
                    }
                }));
            }
        }
        public List<Book> AllBooks {
            get => _allBooks;
            set => Set(ref _allBooks, value);
        }

        public BasketModel BasketModelContext { get; set; } = new BasketModel();
        public string SearchText
        {
            get => _searchText;
            set => Set(ref _searchText, value);
        }
        public ViewModel ShowingViewModel
        {
            get { return _ShowingViewModel; }
            set => Set(ref _ShowingViewModel, value);
        }
        public Action CloseAction { get; set; }
    }
}