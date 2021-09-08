using System;
using System.Globalization;
using System.Windows.Data;

namespace MSFootball.Converters
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is DateTime dt) return dt.ToString(parameter.ToString());
                else return value;
            }
            catch// (Exception ex)
            {
                //TODO: Logger.Error(ex, $"value: {value}, targetType: {targetType}, parameter: {parameter}");
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
