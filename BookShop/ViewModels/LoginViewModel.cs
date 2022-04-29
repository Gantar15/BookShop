using BookShop.Models;
using BookShop.ViewModels.Base;
using DataAccess;

namespace BookShop.ViewModels
{
    class LoginViewModel : ViewModel
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        LoginForm Form = new LoginForm();
    }
}
