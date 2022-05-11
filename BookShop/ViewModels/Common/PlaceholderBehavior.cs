using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text.RegularExpressions;

namespace BookShop.ViewModels.Common
{
	public class PlaceholderBehavior : Behavior<TextBox>
	{
		public static readonly DependencyProperty PlaceholderProperty =
			DependencyProperty.Register("Placeholder", typeof(string), typeof(PlaceholderBehavior), new PropertyMetadata(default(string)));
		public static readonly DependencyProperty PlaceholderColorProperty =
			DependencyProperty.Register("PlaceholderColor", typeof(string), typeof(PlaceholderBehavior), new PropertyMetadata(default(string)));

		public string Placeholder
		{
			get { return (string)GetValue(PlaceholderProperty); }
			set { SetValue(PlaceholderProperty, value); }
		}
		public string PlaceholderColor
		{
			get { return (string)GetValue(PlaceholderColorProperty); }
			set { SetValue(PlaceholderColorProperty, value); }
		}
		private Brush Foreground { get; set; }

		protected void ApplyPlaceholder()
        {
			if (AssociatedObject.IsFocused) return;
			AssociatedObject.Text = Placeholder;
			byte r, g, b;
			(r, g, b) = ParseRgb(PlaceholderColor);
			AssociatedObject.Foreground = new SolidColorBrush(Color.FromRgb(r, g, b));
		}
		protected void ApplyText(string text)
		{
			AssociatedObject.Text = text;
			AssociatedObject.Foreground = Foreground;
		}
		protected (byte r, byte g, byte b) ParseRgb(string colorString)
		{
			(byte r, byte g, byte b) rgb = (154, 154, 154);
			Match rgbMatch = Regex.Match(colorString, @"^((\d{1,3}), ?){2}(\d{1,3})$");
			if (rgbMatch.Success)
			{
				rgb.r = System.Convert.ToByte(rgbMatch.Groups[2].Captures[0].Value);
				rgb.g = System.Convert.ToByte(rgbMatch.Groups[2].Captures[1].Value);
				rgb.b = System.Convert.ToByte(rgbMatch.Groups[3].Value);
			}
			return rgb;
		}

		protected override void OnAttached()
		{
			Foreground = AssociatedObject.Foreground;
			ApplyPlaceholder();

			AssociatedObject.GotFocus += TextBox_Focus;
			AssociatedObject.LostFocus += TextBox_Unfocus;
		}

		protected override void OnDetaching()
		{
			AssociatedObject.GotFocus -= TextBox_Focus;
			AssociatedObject.LostFocus -= TextBox_Unfocus;
		}

		private void TextBox_Focus(object sender, RoutedEventArgs e)
        {
			if (AssociatedObject.Text == Placeholder)
				ApplyText("");
		}
		private void TextBox_Unfocus(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(AssociatedObject.Text))
				ApplyPlaceholder();
		}
	}
}
