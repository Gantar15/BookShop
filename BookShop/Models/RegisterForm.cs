using Kinoa.ViewModel.Validation;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class RegisterForm : ValidateModel
    {
        private string _login;
        private string _email;
        private string _password;

        [Required]
        [RegularExpression(@"^[a-z0-9_-]{3,16}$", ErrorMessage = "Буквы, цифры, дефисы и подчёркивания, от 3 до 16 символов")]
        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        [Required]
        [RegularExpression(@"^([a-z0-9_\.-]+)@([a-z0-9_\.-]+)\.([a-z\.]{2,6})$", ErrorMessage = "Невалидный Email")]
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        [Required]
        [RegularExpression(@"^[a-z0-9_-]{3,16}$", ErrorMessage = "Буквы, цифры, дефисы и подчёркивания, от 3 до 16 символов")]
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
