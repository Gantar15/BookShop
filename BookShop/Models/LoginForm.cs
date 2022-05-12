using BookShop.Models.Validation;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class LoginForm : ValidateModel
    {
        private string _login;
        private string _password;

        public LoginForm()
        {
            Validate();
        }

        [Required(ErrorMessage = "Логин обязателен")]
        public string Login
        {
            get => _login;
            set => Set(ref _login, value);
        }

        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }
    }
}
