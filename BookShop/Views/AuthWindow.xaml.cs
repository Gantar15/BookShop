using BookShop.ViewModels;
using System;
using System.Windows;

namespace BookShop.Views
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        private readonly AuthViewModel _vm;
        public AuthWindow(AuthViewModel vm)
        {
            DataContext = vm;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => Close());
            _vm = vm;
            InitializeComponent();
        }
        public AuthViewModel VM { get { return _vm; } }
    }
}
