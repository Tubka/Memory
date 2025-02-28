using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Memory.Converters
{
    public class MonthHighlightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int selectedMonth && parameter is string buttonMonthStr && int.TryParse(buttonMonthStr, out int buttonMonth))
            {
                return selectedMonth == buttonMonth ? Brushes.LightBlue : Brushes.Transparent;
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
