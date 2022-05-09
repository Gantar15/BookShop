using BookShop.Models.Base;
using DataAccess;

namespace BookShop.Models
{
    public class BasketProductInfo : Model
    {
        private int _count;
        private decimal _totalСost;

        public Book Book { get; set; }
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                TotalСost = _count * Book.Product.Price;
                OnPropertyChanged();
            }
        }
        public decimal TotalСost
        {
            get => _totalСost;
            set
            {
                _totalСost = value;
                OnPropertyChanged();
            }
        }
    }
}
