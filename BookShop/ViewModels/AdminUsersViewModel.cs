using BookShop.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.ViewModels
{
    public class AdminUsersViewModel : ViewModel
    {
        private AdminViewModel _main;

        public AdminUsersViewModel(AdminViewModel main)
        {
            _main = main;
        }
    }
}
