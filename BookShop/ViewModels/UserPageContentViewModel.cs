using BookShop.Infrastructure.Commands;
using BookShop.Models;
using BookShop.Services;
using BookShop.ViewModels.Base;
using BookShop.ViewModels.Common;
using DataAccess;
using System.Collections.ObjectModel;
using System.IO;

namespace BookShop.ViewModels
{
    class UserPageContentViewModel : ViewModel
    {
        private HomeViewModel _main;
        private LambdaCommand _changeImageCommand;
        private LambdaCommand _editUserCommand;
        private readonly MessageBoxService _messageBoxService;
        

        public UserPageContentViewModel(HomeViewModel _main)
        {
            this._main = _main;
            _messageBoxService = new MessageBoxService();

            InitUserForm();
        }

        public void InitUserForm()
        {
            CurrentUser = _main.db.Users.Get(LoggedinUser.Id);
            ProfileFormModel.Email = CurrentUser.Email;
            ProfileFormModel.Image = CurrentUser.Image;
            ProfileFormModel.Login = CurrentUser.Login;
            ProfileFormModel.Name = CurrentUser.Name;
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
                                System.Windows.MessageBoxButton.OK,
                                System.Windows.MessageBoxImage.Warning);
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
                                    System.Windows.MessageBoxButton.OK,
                                    System.Windows.MessageBoxImage.Information);
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
                                    System.Windows.MessageBoxButton.OK,
                                    System.Windows.MessageBoxImage.Information);
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
                                        System.Windows.MessageBoxButton.OK,
                                        System.Windows.MessageBoxImage.Information);
                    }
                }, (o) => !ProfileFormModel.HasErrors));
            }
        }
        public User CurrentUser { get; set; }
        public ObservableCollection<BasketProductInfo> OrderItems { get; set; }
        public UploadPicture UploadPictureService { get; set; } = new UploadPicture();
        public ProfileForm ProfileFormModel { get; set; } = new ProfileForm();
    }
}
