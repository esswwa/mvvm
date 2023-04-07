namespace mvvm.Assets.Converters
{
    public class StringToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as int?;
            if (str == 0) { return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFAFA")); }
            else  if (str < 5 && str > 0) { return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#498c51")); }
            else if (str > 4 && str < 9) { return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7fff00"));}
            else { return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#76e383")); }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
