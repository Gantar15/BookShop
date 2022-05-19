using BookShop.Infrastructure.Commands;
using BookShop.Models;
using BookShop.Services;
using BookShop.ViewModels.Base;
using BookShop.Views;
using DataAccess;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace BookShop.ViewModels
{
    public class AdminUsersViewModel : ViewModel
    {
        private AdminViewModel _main;
        private ObservableCollection<AdminUserForm> _allUsers;
        private LambdaCommand _updateUserCommand;
        private LambdaCommand _removeUserCommand;
        private LambdaCommand _addUserCommand;
        private LambdaCommand _changeImageCommand;
        private LambdaCommand _openAddUserWndCommand;
        private AdminUserForm _newUser;
        private readonly MessageBoxService _messageBoxService = new MessageBoxService();
        private readonly UploadPicture _uploadPictureService = new UploadPicture();

        public AdminUsersViewModel(AdminViewModel main)
        {
            _main = main;
            ResetAllUsers();
            ResetNewUser();
        }

        public void ResetNewUser()
        {
            NewUser = new(@"\Imgs\bg.jpg", _main.db.Roles.GetFirstOrDefault(r => r.Role1 == "User"));
        }
        public void ResetAllUsers()
        {
            AllUsers = new();
            foreach (var user in _main.db.Users.Get(u => u.Role.Role1 != "Admin"))
            {
                var userForm = new AdminUserForm()
                {
                    User = user,
                    Image = user.Image,
                    Password = ""
                };
                AllUsers.Add(userForm);
            }
        }

        public bool SetUserFields(AdminUserForm user)
        {
            if (String.IsNullOrEmpty(user.Image) || String.IsNullOrEmpty(user.User.Login)
                            || String.IsNullOrEmpty(user.User.Name) || String.IsNullOrEmpty(user.User.Email))
            {
                _messageBoxService.ShowMessageBox(
                    user.User.Login,
                    "Все поля должны быть заполнены",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return false;
            }

            var client = _main.db.Users.GetFirstOrDefault(c => c.Login == user.User.Login && c.Id != user.User.Id);
            var emailCandidat = _main.db.Users.Get(c => c.Email == user.User.Email && c.Id != user.User.Id);
            var nameCandidat = _main.db.Users.Get(c => c.Name == user.User.Name && c.Id != user.User.Id);

            if (client != null)
            {
                _messageBoxService.ShowMessageBox(
                            user.User.Login,
                            "Пользователь с таким логином уже существует",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                return false;
            }
            else if (emailCandidat != null)
            {
                _messageBoxService.ShowMessageBox(
                        user.User.Login,
                        "Пользователь с такой почтой уже существует",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                return false;
            }
            else if (nameCandidat != null)
            {
                _messageBoxService.ShowMessageBox(
                        user.User.Login,
                        "Пользователь с таким именем уже существует",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                return false;
            }

            if (!String.IsNullOrEmpty(user.Password))
                user.User.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            if (user.Image != user.User.Image)
                user.User.Image = user.Image;

            return true;
        }

        public AddUserWindow AddUserWnd { get; set; }
        public ObservableCollection<AdminUserForm> AllUsers
        {
            get => _allUsers;
            set => Set(ref _allUsers, value);
        }
        public AdminUserForm NewUser
        {
            get => _newUser;
            set => Set(ref _newUser, value);
        }
        public LambdaCommand UpdateUserCommand
        {
            get
            {
                return _updateUserCommand ?? (_updateUserCommand = new LambdaCommand((o) =>
                {
                    var user = o as AdminUserForm;
                    if (user != null)
                    {
                        if (!SetUserFields(user)) return;

                        _main.db.Users.Update(user.User);
                        _messageBoxService.ShowMessageBox(
                          user.User.Login,
                          $"{user.User.Name} обновлен",
                          MessageBoxButton.OK,
                          MessageBoxImage.Information);
                    }
                }));
            }
        }
        public LambdaCommand RemoveUserCommand
        {
            get
            {
                return _removeUserCommand ?? (_removeUserCommand = new LambdaCommand((o) =>
                {
                    var user = o as AdminUserForm;
                    if (user != null)
                    {
                        _main.db.Users.Remove(user.User.Id);
                        AllUsers.Remove(user);
                    }
                }));
            }
        }
        public LambdaCommand OpenAddUserWndCommand
        {
            get
            {
                return _openAddUserWndCommand ?? (_openAddUserWndCommand = new LambdaCommand((o) =>
                {
                    AddUserWnd = new AddUserWindow(this);
                    AddUserWnd.Show();
                }));
            }
        }
        public LambdaCommand AddUserCommand
        {
            get
            {
                return _addUserCommand ?? (_addUserCommand = new LambdaCommand((o) =>
                {
                    var user = o as AdminUserForm;
                    if (user != null)
                    {
                        if (String.IsNullOrEmpty(user.Password))
                        {
                            _messageBoxService.ShowMessageBox(
                                user.User.Login,
                                "Все поля должны быть заполнены",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                            return;
                        }

                        if (!SetUserFields(user)) return;

                        _main.db.Users.Add(user.User);

                        var userBasket = new Basket
                        {
                            User = user.User
                        };
                        _main.db.Baskets.Add(userBasket);

                        AllUsers.Add(user);
                        ResetNewUser();
                        AddUserWnd.Close();
                    }
                }));
            }
        }
        public LambdaCommand ChangeImageCommand
        {
            get
            {
                return _changeImageCommand ?? (_changeImageCommand = new LambdaCommand(async (o) =>
                {
                    var user = o as AdminUserForm;
                    if (user != null)
                    {
                        var path = _uploadPictureService.OpenFileDialog();
                        if (!string.IsNullOrEmpty(path))
                        {
                            try
                            {
                                var endPath = await _uploadPictureService.AddClientImageAsync(path, user.User.Id);
                                user.Image = endPath;
                            }
                            catch (IOException ex)
                            {
                                _messageBoxService.ShowMessageBox(
                                    user.User.Login,
                                    ex.Message,
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                            }
                        }
                    }
                }));
            }
        }
    }
}
