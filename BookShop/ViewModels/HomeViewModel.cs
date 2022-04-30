using BookShop.Infrastructure.Commands;
using BookShop.ViewModels.Base;
using DataAccess;
using System;

namespace BookShop.ViewModels
{
    public class HomeViewModel : ViewModel
    {
        private ViewModel _ShowingViewModel;
        private readonly UnitOfWork _unitOfWork;

        public HomeViewModel(UnitOfWork unitOfWork = null)
        {
            if (unitOfWork == null)
                _unitOfWork = new UnitOfWork();
            else
                _unitOfWork = unitOfWork;

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
        }

        public LambdaCommand ChangeCommand { get; set; }
        public UnitOfWork db { get => _unitOfWork; }
        public Action CloseAction { get; set; }
        public ViewModel ShowingViewModel
        {
            get { return _ShowingViewModel; }
            set => Set(ref _ShowingViewModel, value);
        }
    }
}