using System;
using System.Globalization;

namespace RustServerMaster.UI.Helpers
{
    /// <summary>
    /// Converts a boolean to its inverse. Use for binding IsEnabled = !SomeBool.
    /// </summary>
    public class InverseBoolConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            // Explicitly use WPF's Binding.DoNothing
            return System.Windows.Data.Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return System.Windows.Data.Binding.DoNothing;
        }
    }
}
