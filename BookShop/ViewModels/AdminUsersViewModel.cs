using BookShop.Infrastructure.Commands;
using BookShop.Models;
using BookShop.Services;
using BookShop.ViewModels.Base;
using BookShop.Views;
using DataAccess;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
        private User _newUser;
        private readonly MessageBoxService _messageBoxService = new MessageBoxService();
        private readonly UploadPicture _uploadPictureService = new UploadPicture();

        public AdminUsersViewModel(AdminViewModel main)
        {
            _main = main;
            ResetAllUsers();
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

        public AddUserWindow AddUserWnd { get; set; }
        public ObservableCollection<AdminUserForm> AllUsers
        {
            get => _allUsers;
            set => Set(ref _allUsers, value);
        }
        public User NewUser
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
                        if (String.IsNullOrEmpty(user.Image) || String.IsNullOrEmpty(user.User.Login)
                            || String.IsNullOrEmpty(user.User.Name))
                        {
                            _messageBoxService.ShowMessageBox(
                                user.User.Login,
                                "Все поля должны быть заполнены",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                            return;
                        }

                        if (!String.IsNullOrEmpty(user.Password))
                            user.User.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                        if (user.Image != user.User.Image)
                            user.User.Image = user.Image;

                        _main.db.Users.UpdateAsync(user.User);
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
                        _main.db.Users.RemoveAsync(user.User.Id);
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
                        AddUserWnd.Close();
                        _main.db.Users.AddAsync(user.User);
                        AllUsers.Add(user);
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
