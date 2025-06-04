using System;
using System.Globalization;
using System.Windows.Data;

namespace RustServerMaster.Helpers
{
    /// <summary>
    /// Converts a boolean to its inverse. Use for binding IsEnabled = !SomeBool.
    /// </summary>
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return Binding.DoNothing!;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return Binding.DoNothing!;
        }
    }
}
