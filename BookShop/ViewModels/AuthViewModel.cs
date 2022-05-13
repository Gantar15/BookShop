using BookShop.Infrastructure.Commands;
using BookShop.Models;
using BookShop.Services;
using BookShop.ViewModels.Base;
using BookShop.ViewModels.Common;
using DataAccess;
using System;
using System.Windows;

namespace BookShop.ViewModels
{
    public class AuthViewModel : ViewModel
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly MessageBoxService _messageBoxService;
        private LambdaCommand _loginCommand;
        private LambdaCommand _registerCommand;

        public AuthViewModel()
        {
            _unitOfWork = new UnitOfWork();
            _messageBoxService = new MessageBoxService();
        }
        public Action CloseAction { get; set; }
        public bool IsLoggedIn { get; set; } = false;

        public UnitOfWork db { get => _unitOfWork; }
        public LoginForm LoginFormModel { get; } = new LoginForm();
        public RegisterForm RegisterFormModel { get; } = new RegisterForm();

        public LambdaCommand LoginCommand
        {
            get
            {
                return _loginCommand ?? (_loginCommand = new LambdaCommand((o) =>
                {
                    var client = db.Users.GetFirstOrDefault(c => c.Login == LoginFormModel.Login);

                    if (client is null)
                    {
                        _messageBoxService.ShowMessageBox(
                                    "Вход",
                                    "Логин введен неверно",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                    }
                    else
                    {
                        if (BCrypt.Net.BCrypt.Verify(LoginFormModel.Password, client.Password))
                        {
                            SetLoggedinUser(client);
                        }
                        else
                        {
                            _messageBoxService.ShowMessageBox(
                                    "Вход",
                                    "Пароль введен неверно",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                        }
                    }

                }, (o) => !LoginFormModel.HasErrors));
            }
        }

        public LambdaCommand RegisterCommand
        {
            get
            {
                return _registerCommand ?? (_registerCommand = new LambdaCommand((o) =>
                {
                    if (!string.IsNullOrEmpty(RegisterFormModel.Login) && !string.IsNullOrEmpty(RegisterFormModel.Email))
                    {
                        var client = db.Users.GetFirstOrDefault(c => c.Login == RegisterFormModel.Login);
                        var emailCandidat = db.Users.Get(c => c.Email == RegisterFormModel.Email);
                        var nameCandidat = db.Users.Get(c => c.Name == RegisterFormModel.Name);

                        if (client != null)
                        {
                            _messageBoxService.ShowMessageBox(
                                        "Регистрация",
                                        "Пользователь с таким логином уже существует",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Information);
                        }
                        else if (emailCandidat != null)
                        {
                            _messageBoxService.ShowMessageBox(
                                    "Регистрация",
                                    "Пользователь с такой почтой уже существует",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                        }
                        else if (nameCandidat != null)
                        {
                            _messageBoxService.ShowMessageBox(
                                    "Регистрация",
                                    "Пользователь с таким именем уже существует",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                        }
                        else
                        {
                            var clientNew = new User
                            {
                                Login = RegisterFormModel.Login,
                                Name = RegisterFormModel.Name,
                                Email = RegisterFormModel.Email,
                                Role = db.Roles.GetFirstOrDefault(r => r.Role1 == "User"),
                                Image = "/Imgs/bg.jpg",
                                Password = BCrypt.Net.BCrypt.HashPassword(RegisterFormModel.Password)
                            };
                            db.Users.Add(clientNew);

                            var userBasket = new Basket
                            {
                                User = clientNew
                            };
                            db.Baskets.Add(userBasket);
                            SetLoggedinUser(clientNew);
                        }
                    }
                }, (o) => !RegisterFormModel.HasErrors));
            }
        }

        private void SetLoggedinUser(User client)
        {
            LoggedinUser.Id = client.Id;
            LoggedinUser.Image = client.Image;
            LoggedinUser.Login = client.Login;
            LoggedinUser.Email = client.Email;
            LoggedinUser.Name = client.Name;
            IsLoggedIn = true;
            CloseAction();
        }
    }
}