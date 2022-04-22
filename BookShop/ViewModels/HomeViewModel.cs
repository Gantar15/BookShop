using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShop.ViewModels.Base;

namespace BookShop.ViewModels
{
    internal class HomeViewModel : ViewModel
    {
        private string _Title;
        public string Title
        { 
            get => _Title; 
            set => Set(ref _Title, value);
        }
    }
}
