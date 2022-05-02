using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BookShop.Infrastructure.Converters
{
    public class InStockToColorConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            var inStock = (bool)value;
            if (inStock)
                return "Green";
            else
                return "Red";
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
