using BookShop.Infrastructure.Commands;
using BookShop.Models;
using BookShop.ViewModels.Base;
using DataAccess;
using System;

namespace BookShop.ViewModels
{
    public class AuthViewModel : ViewModel
    {
        private readonly UnitOfWork _unitOfWork;

        public AuthViewModel()
        {
            _unitOfWork = new UnitOfWork();
        }
        public UnitOfWork db { get => _unitOfWork; }
        public LoginForm LoginFormModel { get; set; } = new LoginForm();
        public RegisterForm RegisterFormModel { get; set; } = new RegisterForm();
        public Action CloseAction { get; set; }
        public bool IsLoggedIn { get; set; } = false;
    }
}