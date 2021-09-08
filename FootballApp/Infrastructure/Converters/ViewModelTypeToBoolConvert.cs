using FootballApp.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace FootballApp.Infrastructure.Converters
{
    class ViewModelTypeToBoolConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return null;
            var tV = value.GetType();
            if (tV.BaseType != typeof(ViewModelBase)) return null;
            if (parameter is null) return null;
            return tV.Name == parameter.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
