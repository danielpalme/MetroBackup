
namespace Palmmedia.BackUp.UI.Wpf.Common
{
    /// <summary>
    /// EBC dependency.
    /// </summary>
    /// <typeparam name="T">The dependant type.</typeparam>
    internal interface IDependsOn<T>
    {
        /// <summary>
        /// Injects the specified type.
        /// </summary>
        /// <param name="type">The dependant type.</param>
        void Inject(T type);
    }
}
