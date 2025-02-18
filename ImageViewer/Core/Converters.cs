using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ImageViewer.Core
{
    public class IsNotNullConverter : IValueConverter
    {
        public static readonly IValueConverter Instance = new IsNotNullConverter();

        public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StateToMarginConverter : IValueConverter
    {
        public static readonly IValueConverter Instance = new StateToMarginConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = (WindowState)value;

            return state == WindowState.Maximized
                ? new Thickness(7, 7, 7, 47)
                : new Thickness(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}