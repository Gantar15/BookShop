using Kinoa.ViewModel.Validation;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class LoginForm : ValidateModel
    {
        private string _login;
        private string _password;

        public LoginForm()
        {
            Login = string.Empty;
        }

        [Required(ErrorMessage = "Логин обязателен")]
        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
    }
}
