using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BookShop.Infrastructure.Converters
{
    public class MoneyConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            var money = (Decimal)value;
            return String.Format("{0:C1}", money);
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
