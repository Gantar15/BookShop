using BookShop.Models.Validation;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class RegisterForm : ValidateModel
    {
        private string _login;
        private string _email;
        private string _password;

        public RegisterForm()
        {
            Validate();
        }

        [Required(ErrorMessage = "Логин обязателен")]
        [RegularExpression(@"^[А-Яа-яA-Za-z0-9_-]{3,16}$", ErrorMessage = "Буквы, цифры, дефисы и подчёркивания, от 3 до 16 символов")]
        public string Login
        {
            get => _login;
            set => Set(ref _login, value);
        }
        [Required(ErrorMessage = "Почта обязательна")]
        [RegularExpression(@"^([A-Za-z0-9_\.-]+)@([A-Za-z0-9_\.-]+)\.([A-Za-z\.]{2,6})$", ErrorMessage = "Некорректная почта")]
        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }
        [Required(ErrorMessage = "Пароль обязателен")]
        [RegularExpression(@"^[a-z0-9_-]{3,16}$", ErrorMessage = "Буквы, цифры, дефисы и подчёркивания, от 3 до 16 символов")]
        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }
    }
}
