using BookShop.Models.Base;
using DataAccess;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BookShop.Models
{
    public class AdminBookForm : Model
    {
        private string _authors;
        private decimal _price;
        private string _category;
        private ObservableCollection<Photo> _photos;
        private Book _book;

        public Book Book {
            get => _book;
            set => Set(ref _book, value);
        }
        public ObservableCollection<Photo> Photos
        {
            get => _photos;
            set => Set(ref _photos, value);
        }
        public string Authors
        {
            get => _authors;
            set => Set(ref _authors, value);
        }
        public decimal Price
        {
            get => _price;
            set => Set(ref _price, value);
        }
        public string Category
        {
            get => _category;
            set => Set(ref _category, value);
        }
    }
}
