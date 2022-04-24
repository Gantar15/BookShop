using BookShop.ViewModels.Base;
using DataAccess;
using System.Linq;
using System.Windows;

namespace BookShop.ViewModels
{
    internal class HomeViewModel : ViewModel
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        private string _Title;
        public string Title
        {
            get => _Title; 
            set => Set(ref _Title, value);
        }
    }
}
