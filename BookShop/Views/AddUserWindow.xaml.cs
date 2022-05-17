using BookShop.ViewModels;
using System.Windows;

namespace BookShop.Views
{
    /// <summary>
    /// Логика взаимодействия для AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        public AddUserWindow(AdminUsersViewModel vm)
        {
            this.DataContext = vm;
            InitializeComponent();
        }
    }
}
