using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BookShop.Infrastructure.Converters
{
    class InStockToStringConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            var inStock = (bool)value;
            if (inStock)
                return "В наличии";
            else
                return "Нет в наличии";
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
