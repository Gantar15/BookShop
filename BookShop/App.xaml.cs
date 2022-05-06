using BookShop.ViewModels;
using BookShop.ViewModels.Common;
using BookShop.Views;
using System.Windows;

namespace BookShop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private AuthWindow AuthWnd;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Current.MainWindow = new AuthWindow(new AuthViewModel());
            AuthWnd = Current.MainWindow as AuthWindow;
            Current.MainWindow.Closing += AuthWindow_Closing;
            Current.MainWindow.Show();
        }

        private void AuthWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var view = Current.MainWindow as AuthWindow;
            if (view is null || !view.VM.IsLoggedIn)
            {
                return;
            }
            Current.MainWindow = new Home(new HomeViewModel(view.VM.db));
            Current.MainWindow.Closing -= AuthWindow_Closing;
            Current.MainWindow.Show();
        }
    }
}
