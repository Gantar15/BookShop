using BookShop.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.ViewModels
{
    public class AdminCategoriesViewModel : ViewModel
    {
        private AdminViewModel _main;

        public AdminCategoriesViewModel(AdminViewModel main)
        {
            _main = main;
        }
    }
}
