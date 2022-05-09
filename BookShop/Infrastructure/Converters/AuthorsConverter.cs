using DataAccess;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BookShop.Infrastructure.Converters
{
    public class AuthorsConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            var authors = value as List<Author>;
            if (authors != null)
            {
                string resultAuthors = "";
                for (int i = 0; i < authors.Count; i++)
                {
                    var author = authors[i];
                    resultAuthors += $"{author.Name} {author.Surname}";
                    if (i < authors.Count - 1)
                        resultAuthors += ", ";
                }
                return resultAuthors;
            }
            return "";
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
