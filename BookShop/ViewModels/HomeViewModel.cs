using BookShop.Infrastructure.Commands;
using BookShop.ViewModels.Base;
using DataAccess;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BookShop.ViewModels
{
    public class HomeViewModel : ViewModel
    {
        private ViewModel _ShowingViewModel;
        private string _Title;
        UnitOfWork unitOfWork = new UnitOfWork();

        public HomeViewModel()
        {
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

        public string Title
        {
            get => _Title; 
            set => Set(ref _Title, value);
        }
        public ViewModel ShowingViewModel
        {
            get { return _ShowingViewModel; }
            set => Set(ref _ShowingViewModel, value);
        }
    }
}
