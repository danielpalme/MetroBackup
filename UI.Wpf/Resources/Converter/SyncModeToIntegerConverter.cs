using System;
using System.Globalization;
using System.Windows.Data;
using Palmmedia.BackUp.Synchronization.SyncModes;

namespace Palmmedia.BackUp.UI.Wpf.Resources.Converter
{
    /// <summary>
    /// Converts a <see cref="SyncModeType"/> to an Interger and vise versa.
    /// </summary>
    public sealed class SyncModeTypeToIntegerConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="SyncModeType"/> to its corressponding Integer.
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
            return (int)(SyncModeType)value;
        }

        /// <summary>
        /// Converts a Integer to its corressponding <see cref="SyncModeType"/>.
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
            return (SyncModeType)value;
        }
    }
}
