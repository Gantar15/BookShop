using BookShop.ViewModels;
using System.Windows;
namespace BookShop.Views
{
    /// <summary>
    /// Логика взаимодействия для AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        public AddBookWindow(AdminBooksViewModel vm)
        {
            this.DataContext = vm;
            InitializeComponent();
        }
    }
}
