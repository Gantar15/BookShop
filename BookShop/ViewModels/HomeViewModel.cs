using BookShop.Infrastructure.Commands;
using BookShop.Models;
using BookShop.ViewModels.Base;
using BookShop.ViewModels.Common;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BookShop.ViewModels
{
    public class HomeViewModel : ViewModel
    {
        private ViewModel _ShowingViewModel;
        private readonly UnitOfWork _unitOfWork;
        private string _searchText;
        private List<Book> _allBooks;
        private LambdaCommand _searchCommand;

        public HomeViewModel(UnitOfWork unitOfWork = null)
        {
            if (unitOfWork == null)
                _unitOfWork = new UnitOfWork();
            else
                _unitOfWork = unitOfWork;

            AllBooks = db.Books.Items.ToList();
            ShowingViewModel = new HomeContentViewModel(this);
            ChangeCommand = new LambdaCommand((name) =>
            {
                switch (name.ToString())
                {
                    case "home":
                        ShowingViewModel = new HomeContentViewModel(this);
                        break;
                    case "contacts":
                        ShowingViewModel = new ContactsVeiwModel();
                        break;
                    case "deliveryInfo":
                        ShowingViewModel = new DeliveryInfoVeiwModel();
                        break;
                }
            });
            UpdateBasket();
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
        public LambdaCommand ChangeCommand { get; set; }
        public LambdaCommand SearchCommand
        {
            get {
                return _searchCommand ?? (_searchCommand = new LambdaCommand((o) =>
                {
                    AllBooks = db.Books.Get(b => Regex.IsMatch(b.Title, $"^.*{SearchText}.*$"));
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