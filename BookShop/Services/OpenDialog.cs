using BookShop.Views;

namespace BookShop.Services
{
    public class OpenDialog
    {
        public bool? ShowDialog(object DataContext)
        {
            var win = new DialogWindow();
            win.DataContext = DataContext;
            return win.ShowDialog();
        }
    }
}
