using System;
using System.Linq;

namespace Palmmedia.BackUp.UI.Wpf.Resources
{
    /// <summary>
    /// Translates the members of an enum type.
    /// </summary>
    public static class EnumLocalizer
    {
        /// <summary>
        /// Gets the localized enum names.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns>The localized enum names.</returns>
        public static string[] GetLocalizedEnumNames(Type enumType)
        {
            var values = Enum.GetNames(enumType);

            return values.Select(v => Palmmedia.BackUp.SharedResources.Common.ResourceManager.GetString(enumType.Name + "_" + v)).ToArray();
        }
    }
}
