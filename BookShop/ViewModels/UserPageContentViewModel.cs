using BookShop.Infrastructure.Commands;
using BookShop.Infrastructure.Dto;
using BookShop.Models;
using BookShop.Services;
using BookShop.ViewModels.Base;
using BookShop.ViewModels.Common;
using DataAccess;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace BookShop.ViewModels
{
    class UserPageContentViewModel : ViewModel
    {
        private HomeViewModel _main;
        private LambdaCommand _changeImageCommand;
        private LambdaCommand _editUserCommand;
        private LambdaCommand _showBookPage;
        private readonly MessageBoxService _messageBoxService;

        public UserPageContentViewModel(HomeViewModel _main)
        {
            this._main = _main;
            _messageBoxService = new MessageBoxService();

            InitUserForm();
            InitOrders();
        }

        public void InitOrders()
        {
            int userId = LoggedinUser.Id;
            var currentOrders = _main.db.Orders.Get(order => order.UserId == userId);

            foreach (var currentOrder in currentOrders) {
                var orderItemsGroup = new OrderItemsGroup();
                var booksItems = new List<Book>();
                currentOrder.Products.ForEach(p => booksItems.Add(p.Book));

                foreach (var bookitem in booksItems)
                {
                    var basketProductInfo = new BasketProductInfo();
                    var orderProduct = _main.db.OrderProducts.GetFirstOrDefault(bp => bp.ProductId == bookitem.Product.Id && bp.OrderId == currentOrder.Id);
                    basketProductInfo.Book = bookitem;
                    if (orderProduct != null)
                    {
                        basketProductInfo.Count = orderProduct.Count;
                        basketProductInfo.TotalСost = orderProduct.Count * bookitem.Product.Price;
                    }
                    orderItemsGroup.OrderItems.Add(basketProductInfo);
                }
                OrderItemsGroups.Add(orderItemsGroup);
            }
        }
        public void InitUserForm()
        {
            ProfileFormModel.Email = LoggedinUser.Email;
            ProfileFormModel.Image = LoggedinUser.Image;
            ProfileFormModel.Login = LoggedinUser.Login;
            ProfileFormModel.Name = LoggedinUser.Name;
        }

        public LambdaCommand ChangeImageCommand
        {
            get
            {
                return _changeImageCommand ??
                (_changeImageCommand = new LambdaCommand(async (o) =>
                {
                    var path = UploadPictureService.OpenFileDialog();
                    if (!string.IsNullOrEmpty(path))
                    {
                        try
                        {
                            var endPath = await UploadPictureService.AddClientImageAsync(path, LoggedinUser.Id);
                            ProfileFormModel.Image = endPath;
                        }
                        catch (IOException ex)
                        {
                            _messageBoxService.ShowMessageBox(
                                "Профиль",
                                ex.Message,
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                        }
                    }
                }));
            }
        }
        public LambdaCommand EditUserCommand
        {
            get
            {
                return _editUserCommand ??
                (_editUserCommand = new LambdaCommand((o) =>
                {
                    bool HasChanges = false;
                    if (!string.IsNullOrEmpty(ProfileFormModel.Name) && ProfileFormModel.Name != CurrentUser.Name)
                    {
                        var nameCandidat = _main.db.Users.Get(c => c.Name == ProfileFormModel.Name);
                        if (nameCandidat != null)
                        {
                            _messageBoxService.ShowMessageBox(
                                    "Профиль",
                                    "Пользователь с таким именем уже существует",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                        }
                        else
                        {
                            CurrentUser.Name = ProfileFormModel.Name;
                            _main.LoggedinUserName = ProfileFormModel.Name;
                            HasChanges = true;
                        }
                    }
                    if (!string.IsNullOrEmpty(ProfileFormModel.Email) && ProfileFormModel.Email != CurrentUser.Email)
                    {
                        var emailCandidat = _main.db.Users.Get(c => c.Email == ProfileFormModel.Email);
                        if (emailCandidat != null)
                        {
                            _messageBoxService.ShowMessageBox(
                                    "Профиль",
                                    "Пользователь с такой почтой уже существует",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                        }
                        else
                        {
                            CurrentUser.Email = ProfileFormModel.Email;
                            HasChanges = true;
                        }
                    }
                    if (!string.IsNullOrEmpty(ProfileFormModel.Password) && !BCrypt.Net.BCrypt.Verify(ProfileFormModel.Password, CurrentUser.Password))
                    {
                        CurrentUser.Password = BCrypt.Net.BCrypt.HashPassword(ProfileFormModel.Password);
                        HasChanges = true;
                    }
                    if (ProfileFormModel.Image != CurrentUser.Image)
                    {
                        CurrentUser.Image = ProfileFormModel.Image;
                        _main.LoggedinUserImage = ProfileFormModel.Image;
                        HasChanges = true;
                    }

                    if (HasChanges)
                    {
                        _main.db.Users.Update(CurrentUser);
                        _messageBoxService.ShowMessageBox(
                                        "Профиль",
                                        "Ваши данные успешно изменены",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Information);
                    }
                }, (o) => !ProfileFormModel.HasErrors));
            }
        }
        public LambdaCommand ShowBookPage
        {
            get
            {
                return _showBookPage ?? (_showBookPage = new LambdaCommand((o) =>
                {
                    var book = o as Book;
                    if (book != null)
                    {
                        _main.ShowBookPage.Execute(book);
                    }
                }));
            }
        }
        public User CurrentUser { get; set; }
        public ObservableCollection<OrderItemsGroup> OrderItemsGroups { get; set; } = new();
        public UploadPicture UploadPictureService { get; set; } = new UploadPicture();
        public ProfileForm ProfileFormModel { get; set; } = new ProfileForm();
    }
}
