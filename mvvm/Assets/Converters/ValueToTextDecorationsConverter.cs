namespace mvvm.Assets.Converters
{
    public class ValueToTextDecorationsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as int?;
            if (str == null) { return default; }
            else if (str == 0) { return default; }
            else { return TextDecorations.Strikethrough; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
