using System.Windows;

namespace BookShop.Services
{
    public class MessageBoxService
    { 
        public bool ShowMessageBox(string title, string text, MessageBoxButton button, MessageBoxImage image)
        {
            MessageBoxResult result = MessageBox.Show(text, title, button, image);
            return MessageBoxResult.Yes == result;
        }
    }
}
