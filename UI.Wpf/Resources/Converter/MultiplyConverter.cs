using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Palmmedia.BackUp.UI.Wpf.Resources.Converter
{
    /// <summary>
    /// Multiplies the given value with the parameter.
    /// </summary>
    public sealed class MultiplyConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="double"/>  to a <see cref="SolidColorBrush"/>.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (parameter == null)
            {
                return value;
            }

            return (double)value * double.Parse(parameter.ToString(), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a <see cref="SolidColorBrush"/> to a <see cref="Color"/>.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
