using BookShop.Infrastructure.Commands;
using BookShop.Models;
using BookShop.Services;
using BookShop.ViewModels.Base;
using BookShop.ViewModels.Common;
using BookShop.Views;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace BookShop.ViewModels
{
    public class AdminBooksViewModel : ViewModel
    {
        private AdminViewModel _main;
        private ObservableCollection<AdminBookForm> _allBooks;
        private LambdaCommand _updateBookCommand;
        private LambdaCommand _removeBookCommand;
        private LambdaCommand _addImageCommand;
        private LambdaCommand _removeImageCommand;
        private LambdaCommand _addBookCommand;
        private LambdaCommand _openAddBookWndCommand;
        private AdminBookForm _newBookForm;
        private readonly MessageBoxService _messageBoxService = new MessageBoxService();
        private readonly UploadPicture _uploadPictureService = new UploadPicture();

        public AdminBooksViewModel(AdminViewModel main)
        {
            _main = main;
            ResetAllBooks();
            ResetNewBookForm();
        }

        public void ResetNewBookForm()
        {
            NewBookForm = new AdminBookForm();
        }
        public void ResetAllBooks()
        {
            AllBooks = new();
            var books = _main.db.Books.Items.ToList();
            foreach (var book in books)
            {
                var bookModel = new AdminBookForm();
                bookModel.Book = book;
                book.Authors.ForEach(author => bookModel.Authors += author.Name + " " + author.Surname + ", ");
                bookModel.Authors = bookModel.Authors.Substring(0, bookModel.Authors.Length - 2);
                bookModel.Price = book.Product.Price;
                bookModel.Category = book.Category.Title;
                bookModel.Photos = new();
                foreach (var photo in book.Photos.ToList())
                {
                    bookModel.Photos.Add(photo);
                }
                AllBooks.Add(bookModel);
            }
        }
        public List<KeyValuePair<string, string>> ParseAuthors(string authorsString)
        {
            List<KeyValuePair<string, string>> names = new();
            var namesParts = authorsString.Split(",");
            foreach (var namePart in namesParts)
            {
                var nameSername = namePart.Trim().Split(" ");
                if (nameSername.Length != 2) return null;
                KeyValuePair<string, string> pair = new(nameSername[0], nameSername[1]);
                names.Add(pair);
            }

            return names;
        }
        public bool SetBookFields(AdminBookForm book)
        {
            if (String.IsNullOrEmpty(book.Authors) || String.IsNullOrEmpty(book.Book.AgeRestriction)
                        || String.IsNullOrEmpty(book.Category) || String.IsNullOrEmpty(book.Book.Description)
                        || String.IsNullOrEmpty(book.Book.Format) || book.Photos.Count == 0
                        || String.IsNullOrEmpty(book.Book.Title) || String.IsNullOrEmpty(book.Book.Description))
            {
                _messageBoxService.ShowMessageBox(
               book.Book.Title,
               "Все поля должны быть заполнены",
               MessageBoxButton.OK,
               MessageBoxImage.Information);
                return false;
            }

            //Authors
            var authorsNames = ParseAuthors(book.Authors);
            if (authorsNames == null)
            {
                _messageBoxService.ShowMessageBox(
              book.Book.Title,
              "Некорректные авторы",
              MessageBoxButton.OK,
              MessageBoxImage.Information);
                return false;
            }
            book.Book.Authors.Clear();
            foreach (var authorName in authorsNames)
            {
                var authorCandidate = _main.db.Authors.GetFirstOrDefault(a => a.Name == authorName.Key && a.Surname == authorName.Value);
                var hasAuthor = book.Book.Authors.FirstOrDefault(a => a.Name == authorName.Key && a.Surname == authorName.Value);
                if (authorCandidate == null)
                {
                    authorCandidate = new Author
                    {
                        Name = authorName.Key,
                        Surname = authorName.Value
                    };
                    _main.db.Authors.Add(authorCandidate);
                }
                if (hasAuthor == null)
                {
                    book.Book.Authors.Add(authorCandidate);
                }
            }

            //Category
            var categoryCandidate = _main.db.Categories.GetFirstOrDefault(c => c.Title == book.Category);
            if (categoryCandidate == null)
            {
                _messageBoxService.ShowMessageBox(
                 book.Book.Title,
                 "Указана несуществующая категория",
                 MessageBoxButton.OK,
                 MessageBoxImage.Information);
                return false;
            }
            var hasCategory = book.Book.Category.Title == book.Category;
            if (!hasCategory)
            {
                book.Book.Category = categoryCandidate;
            }

            //Price
            if (_main.db.Products.Get(book.Book.Product.Id) == null)
            {
                var newProduct = new Product
                {
                    Price = book.Price
                };
                _main.db.Products.Add(newProduct);
                book.Book.Product = newProduct;
            }
            else
            {
                book.Book.Product.Price = book.Price;
            }

            //Photos
            foreach (var photo in book.Photos)
            {
                if(_main.db.Photos.Get(photo.Id) == null)
                    _main.db.Photos.Add(photo);
                book.Book.Photos.Add(photo);
            }
            foreach (var photo in book.Book.Photos.ToList())
            {
                if (!book.Photos.Contains(photo))
                    _main.db.Photos.Remove(photo.Id);
            }

            return true;
        }

        public AddBookWindow AddBookWnd { get; set; }
        public ObservableCollection<AdminBookForm> AllBooks
        {
            get => _allBooks;
            set => Set(ref _allBooks, value);
        }
        public AdminBookForm NewBookForm
        {
            get => _newBookForm;
            set => Set(ref _newBookForm, value);
        }
        public LambdaCommand UpdateBookCommand
        {
            get
            {
                return _updateBookCommand ?? (_updateBookCommand = new LambdaCommand((o) =>
                {
                    var book = o as AdminBookForm;
                    if (book != null)
                    {
                        if (!SetBookFields(book)) return;

                        _main.db.Books.Update(book.Book);
                        _messageBoxService.ShowMessageBox(
                          book.Book.Title,
                          "Книга обновлена",
                          MessageBoxButton.OK,
                          MessageBoxImage.Information);
                    }
                }));
            }
        }
        public LambdaCommand RemoveBookCommand
        {
            get
            {
                return _removeBookCommand ?? (_removeBookCommand = new LambdaCommand((o) =>
                {
                    var book = o as AdminBookForm;
                    if (book != null)
                    {
                        _main.db.Books.Remove(book.Book.Id);
                        AllBooks.Remove(book);
                    }
                }));
            }
        }
        public LambdaCommand OpenAddBookWndCommand
        {
            get
            {
                return _openAddBookWndCommand ?? (_openAddBookWndCommand = new LambdaCommand((o) =>
                {
                    AddBookWnd = new AddBookWindow(this);
                    AddBookWnd.Show();
                }));
            }
        }
        public LambdaCommand AddBookCommand
        {
            get
            {
                return _addBookCommand ?? (_addBookCommand = new LambdaCommand((o) =>
                {
                    var book = o as AdminBookForm;
                    if (book != null)
                    {
                        if (!SetBookFields(book)) return;

                        _main.db.Books.Add(book.Book);
                        AllBooks.Add(book);
                        ResetNewBookForm();
                        AddBookWnd.Close();
                    }
                }));
            }
        }
        public LambdaCommand AddImageCommand
        {
            get
            {
                return _addImageCommand ??
                (_addImageCommand = new LambdaCommand(async (o) =>
                {
                    var book = o as AdminBookForm;
                    if (book != null)
                    {
                        var path = _uploadPictureService.OpenFileDialog();
                        if (!string.IsNullOrEmpty(path))
                        {
                            try
                            {
                                var endPath = await _uploadPictureService.AddClientImageAsync(path, LoggedinUser.Id);
                                book.Photos.Add(new Photo
                                {
                                    Source = endPath
                                });
                            }
                            catch (IOException ex)
                            {
                                _messageBoxService.ShowMessageBox(
                                    book.Book.Title,
                                    ex.Message,
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                            }
                        }
                    }
                }));
            }
        }
        public LambdaCommand RemoveImageCommand
        {
            get
            {
                return _removeImageCommand ??
                (_removeImageCommand = new LambdaCommand((o) =>
                {
                    var photo = o as Photo;
                    if (photo != null)
                    {
                        foreach (var book in AllBooks)
                        {
                            if (book.Photos.Contains(photo))
                                book.Photos.Remove(photo);
                        }
                        if (NewBookForm != null && NewBookForm.Photos.Contains(photo))
                            NewBookForm.Photos.Remove(photo);
                    }
                }));
            }
        }

    }
}
