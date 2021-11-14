using System;
using System.Globalization;
using System.Windows.Data;

namespace VertaIT.ThemeWPF.Converters
{
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value == null) ? parameter == null : Equals(value, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Type nullableTargetType = Nullable.GetUnderlyingType(targetType);

            if (nullableTargetType != null)
            {
                if (parameter is null)
                {
                    return null;
                }
                else if (parameter.GetType() == nullableTargetType)
                {
                    return parameter;
                }
                else
                {
                    throw new ArgumentException("The parameter type is invalid");
                }
            }
            else
            {
                if (value is null)
                {
                    throw new ArgumentNullException("The value cannot be null");
                }
                else if (parameter is null)
                {
                    throw new ArgumentNullException("The parameter cannot be null");
                }
                else if (parameter.GetType() != targetType)
                {
                    throw new ArgumentException("The parameter type is invalid");
                }
                else if (!(value is bool))
                {
                    throw new ArgumentNullException("The value typye is not a bool");
                }
                else
                {
                    return parameter;
                }
            }
        }
    }
}