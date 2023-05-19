namespace Pishi_Wash__Store.Assets.Converters
{
    class AdminStringToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as ICollection<Orderproduct>;
            if (str == null) { return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#76e383")); }
            else if (str.Any(a => a.ProductArticleNumberNavigation.ProductQuantityInStock > 3)) { return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#20b2aa")); }
            else { return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff8c00")); }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
