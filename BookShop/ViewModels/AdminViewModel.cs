using BookShop.Infrastructure.Commands;
using BookShop.Services;
using BookShop.ViewModels.Base;
using DataAccess;
using System;

namespace BookShop.ViewModels
{
    public class AdminViewModel : ViewModel
    {
        private readonly UnitOfWork _unitOfWork;
        private LambdaCommand _searchCommand;
        public AdminBooksViewModel _adminBooksViewModel;
        public AdminCategoriesViewModel _adminCategoriesViewModel;
        public AdminUsersViewModel _adminUsersViewModel;
        private string _searchText;

        public AdminViewModel(UnitOfWork unitOfWork = null)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            AdminBooksViewModel = new AdminBooksViewModel(this);
            AdminCategoriesViewModel = new AdminCategoriesViewModel(this);
            AdminUsersViewModel = new AdminUsersViewModel(this);
        }

        public Action CloseAction { get; set; }
        public AdminBooksViewModel AdminBooksViewModel {
            get => _adminBooksViewModel;
            set => Set(ref _adminBooksViewModel, value);
        }
        public AdminCategoriesViewModel AdminCategoriesViewModel 
        {
            get => _adminCategoriesViewModel;
            set => Set(ref _adminCategoriesViewModel, value);
        }
        public AdminUsersViewModel AdminUsersViewModel 
        {
            get => _adminUsersViewModel;
            set => Set(ref _adminUsersViewModel, value);
        }
        public UnitOfWork db { get => _unitOfWork; }
        public LambdaCommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new LambdaCommand((o) =>
                {

                }));
            }
        }
        public string SearchText
        {
            get => _searchText;
            set => Set(ref _searchText, value);
        }
    }
}
