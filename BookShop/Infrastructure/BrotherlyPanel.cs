using System.Windows;
using System.Windows.Controls;

namespace BookShop.Infrastructure
{
    public class BrotherlyPanel : WrapPanel
    {
        protected override Size ArrangeOverride(Size finalSize)
        {
            var itemsHeight = finalSize.Height / Children.Count;
            for (var i = 0; i < Children.Count; i++)
                Children[i].Arrange(new Rect(0, i * itemsHeight, finalSize.Width, itemsHeight));
            return finalSize;
        }
    }
}
