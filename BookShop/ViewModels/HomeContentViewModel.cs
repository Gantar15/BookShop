using BookShop.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.ViewModels
{
    public class HomeContentViewModel : ViewModel
    {
        private HomeViewModel _main;
        public HomeContentViewModel(HomeViewModel homeViewModel)
        {
            _main = homeViewModel;
        }
    }
}
