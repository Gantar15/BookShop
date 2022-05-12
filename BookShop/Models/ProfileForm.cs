using BookShop.Models.Validation;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class ProfileForm : ValidateModel
    {
        private string _image;
        private string _email;
        private string _login;
        private string _name;
        private string _password;

        public ProfileForm()
        {
            Validate();
        }

        public string Login
        {
            get => _login;
            set => Set(ref _login, value);
        }

        [RegularExpression(@"^[А-Яа-яA-Za-z0-9_\-]{2,30}$", ErrorMessage = "Буквы, цифры, дефисы и подчёркивания, от 2 до 30 символов")]
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        [RegularExpression(@"^([a-z0-9_\.\- ]+)@([a-z0-9_\.-]+)\.([a-z\.]{2,6})$", ErrorMessage = "Невалидный Email")]
        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }

        [RegularExpression(@"^[a-zA-Z0-9_-]{3,16}$", ErrorMessage = "Буквы, цифры, дефисы и подчёркивания, от 3 до 16 символов")]
        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }
        public string Image
        {
            get => _image;
            set => Set(ref _image, value);
        }
    }
}
