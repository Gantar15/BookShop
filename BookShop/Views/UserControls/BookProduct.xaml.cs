using System.Windows;
using System.Windows.Controls;

namespace BookShop.Views.CustomElements
{
    public partial class BookProduct : UserControl
    {
        public BookProduct()
        {
            InitializeComponent();
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(BookProduct));
    }
}
