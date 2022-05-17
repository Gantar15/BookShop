using BookShop.Models.Base;
using DataAccess;

namespace BookShop.Models
{    public class AdminUserForm : Model
    {
        private string _image;
        private string _password;
        private User _user;

        public User User
        {
            get => _user;
            set => Set(ref _user, value);
        }
        public string Image
        {
            get => _image;
            set => Set(ref _image, value);
        }
        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }
    }
}
