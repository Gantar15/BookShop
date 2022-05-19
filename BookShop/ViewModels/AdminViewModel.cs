using BookShop.Infrastructure.Commands;
using BookShop.ViewModels.Base;
using DataAccess;
using System;

namespace BookShop.ViewModels
{
    public class AdminViewModel : ViewModel
    {
        private readonly UnitOfWork _unitOfWork;
        private AdminPageViewModel _selectedPage;
        private LambdaCommand _selectPageCommand;
        private int _selectedTabControlPageIndex;
        private AdminBooksViewModel _adminBooksViewModel;
        private AdminCategoriesViewModel _adminCategoriesViewModel;
        private AdminUsersViewModel _adminUsersViewModel;
        private string _searchText;

        public AdminViewModel(UnitOfWork unitOfWork = null)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            AdminBooksViewModel = new AdminBooksViewModel(this);
            AdminCategoriesViewModel = new AdminCategoriesViewModel(this);
            AdminUsersViewModel = new AdminUsersViewModel(this);
            _selectedPage = AdminBooksViewModel;
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
        public LambdaCommand SearchCommand
        {
            get => new LambdaCommand((o) => _selectedPage.SearchCommand.Execute(o));
        }
        public LambdaCommand SelectPageCommand
        {
            get
            {
                return _selectPageCommand ??
                (_selectPageCommand = new LambdaCommand((o) =>
                {
                    var page = o as string;
                    if(page != null)
                    {
                        switch (page)
                        {
                            case "Books":
                                _selectedPage = AdminBooksViewModel;
                                SelectedTabControlPageIndex = 0;
                                break;
                            case "Users":
                                _selectedPage = AdminUsersViewModel;
                                SelectedTabControlPageIndex = 1;
                                break;
                            case "Categories":
                                _selectedPage = AdminCategoriesViewModel;
                                SelectedTabControlPageIndex = 2;
                                break;
                        }
                    }
                }));
            }
        }
        public UnitOfWork db { get => _unitOfWork; }
        public string SearchText
        {
            get => _searchText;
            set => Set(ref _searchText, value);
        }
        public int SelectedTabControlPageIndex
        {
            get => _selectedTabControlPageIndex;
            set => Set(ref _selectedTabControlPageIndex, value);
        }
    }
}
