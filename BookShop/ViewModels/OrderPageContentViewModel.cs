using BookShop.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.ViewModels
{
    public class OrderPageContentViewModel : ViewModel
    {
        private HomeViewModel _main;

        public OrderPageContentViewModel(HomeViewModel _main)
        {
            this._main = _main;
        }
    }
}
