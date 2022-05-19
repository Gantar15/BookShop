using BookShop.Infrastructure.Commands;
using BookShop.Models;
using BookShop.Services;
using BookShop.ViewModels.Base;
using BookShop.Views;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace BookShop.ViewModels
{
    public class AdminCategoriesViewModel : AdminPageViewModel
    {
        private AdminViewModel _main;
        private ObservableCollection<AdminCategoryForm> _allCategories;
        private LambdaCommand _updateCategoryCommand;
        private LambdaCommand _removeCategoryCommand;
        private LambdaCommand _addCategoryCommand;
        private LambdaCommand _changeImageCommand;
        private LambdaCommand _openAddCategoryWndCommand;
        private AdminCategoryForm _newCategory;
        private readonly MessageBoxService _messageBoxService = new MessageBoxService();
        private readonly UploadPicture _uploadPictureService = new UploadPicture();

        public AdminCategoriesViewModel(AdminViewModel main)
        {
            _main = main;
            ResetAllCategories();
            ResetNewCategory();
        }

        public void ResetNewCategory()
        {
            NewCategory = new();
        }
        public void ResetAllCategories()
        {
            AllCategories = new();
            foreach (var category in _main.db.Categories.Items.ToList())
            {
                var categoryForm = new AdminCategoryForm()
                {
                    Category = category,
                    Image = category.Image
                };
                AllCategories.Add(categoryForm);
            }
        }

        public bool SetCategoryFields(AdminCategoryForm category)
        {
            if (String.IsNullOrEmpty(category.Image) || String.IsNullOrEmpty(category.Category.Title))
            {
                _messageBoxService.ShowMessageBox(
                    category.Category.Title,
                    "Все поля должны быть заполнены",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return false;
            }

            var categoryCandidate = _main.db.Categories.GetFirstOrDefault(c => c.Title == category.Category.Title && c.Id != category.Category.Id);
            if (categoryCandidate != null)
            {
                _messageBoxService.ShowMessageBox(
                            category.Category.Title,
                            "Категория с таким названием уже существует",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                return false;
            }

            category.Category.Image = category.Image;

            return true;
        }

        public AddCategoryWindow AddCategoryWnd { get; set; }
        public ObservableCollection<AdminCategoryForm> AllCategories
        {
            get => _allCategories;
            set => Set(ref _allCategories, value);
        }
        public AdminCategoryForm NewCategory
        {
            get => _newCategory;
            set => Set(ref _newCategory, value);
        }
        public override LambdaCommand _searchCommand { get; set; }
        public LambdaCommand UpdateCategoryCommand
        {
            get
            {
                return _updateCategoryCommand ?? (_updateCategoryCommand = new LambdaCommand((o) =>
                {
                    var category = o as AdminCategoryForm;
                    if (category != null)
                    {
                        if (!SetCategoryFields(category)) return;

                        _main.db.Categories.Update(category.Category);
                        _messageBoxService.ShowMessageBox(
                          category.Category.Title,
                          $"{category.Category.Title} обновлен",
                          MessageBoxButton.OK,
                          MessageBoxImage.Information);
                    }
                }));
            }
        }
        public LambdaCommand RemoveCategoryCommand
        {
            get
            {
                return _removeCategoryCommand ?? (_removeCategoryCommand = new LambdaCommand((o) =>
                {
                    var category = o as AdminCategoryForm;
                    if (category != null)
                    {
                        var categoryBookCandidate = _main.db.Books.GetFirstOrDefault(b => b.CategoryId == category.Category.Id);
                        if (categoryBookCandidate != null)
                        {
                            _messageBoxService.ShowMessageBox(
                              category.Category.Title,
                              $"Нельзя удалить категорию, если некоторые книги относятся к ней",
                              MessageBoxButton.OK,
                              MessageBoxImage.Information);
                            return;
                        }

                        _main.db.Categories.Remove(category.Category.Id);
                        AllCategories.Remove(category);
                    }
                }));
            }
        }
        public LambdaCommand OpenAddCategoryWndCommand
        {
            get
            {
                return _openAddCategoryWndCommand ?? (_openAddCategoryWndCommand = new LambdaCommand((o) =>
                {
                    AddCategoryWnd = new AddCategoryWindow(this);
                    AddCategoryWnd.Show();
                }));
            }
        }
        public LambdaCommand AddCategoryCommand
        {
            get
            {
                return _addCategoryCommand ?? (_addCategoryCommand = new LambdaCommand((o) =>
                {
                    var category = o as AdminCategoryForm;
                    if (category != null)
                    {
                        if (!SetCategoryFields(category)) return;

                        _main.db.Categories.Add(category.Category);
                        AllCategories.Add(category);
                        ResetNewCategory();
                        AddCategoryWnd.Close();
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
                    var category = o as AdminCategoryForm;
                    if (category != null)
                    {
                        var path = _uploadPictureService.OpenFileDialog();
                        if (!string.IsNullOrEmpty(path))
                        {
                            try
                            {
                                var endPath = await _uploadPictureService.AddClientImageAsync<Category>(path, category.Category.Id);
                                category.Image = endPath;
                            }
                            catch (IOException ex)
                            {
                                _messageBoxService.ShowMessageBox(
                                    category.Category.Title,
                                    ex.Message,
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                            }
                        }
                    }
                }));
            }
        }
        public override LambdaCommand SearchCommand
        {
            get
            {
                return _searchCommand ??
                (_searchCommand = new LambdaCommand((o) =>
                {
                    if (!String.IsNullOrEmpty(_main.SearchText))
                    {
                        List<Category> searchResults = new();
                        var titleSearch = _main.db.Categories.Get(c => Regex.IsMatch(c.Title, $"^.*{_main.SearchText}.*$", RegexOptions.IgnoreCase));

                        if (titleSearch != null)
                            searchResults.AddRange(titleSearch);

                        searchResults = searchResults.Select(b => b).Distinct().ToList();

                        AllCategories.Clear();
                        foreach (var searchResult in searchResults)
                        {
                            AllCategories.Add(new AdminCategoryForm()
                            {
                                Image = searchResult.Image,
                                Category = searchResult
                            });
                        }
                    }
                    else
                    {
                        ResetAllCategories();
                    }
                }));
            }
        }

    }
}
