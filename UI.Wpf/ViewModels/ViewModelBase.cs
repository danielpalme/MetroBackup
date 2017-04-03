using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace Palmmedia.BackUp.UI.Wpf.ViewModels
{
    /// <summary>
    /// Base class for all ViewModel classes in the application.
    /// It provides support for property change notifications.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        protected ViewModelBase()
        {
        }

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invoked when this object is being removed from the application
        /// and will be subject to garbage collection.
        /// Do not make this method virtual.
        /// A derived class should not be able to override this method.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Child classes can override this method to perform 
        /// clean-up logic, such as removing event handlers.
        /// If <paramref name="disposeManagedResources"/> equals <c>True</c>, all managed resources should be disposed.
        /// Both managed and unmanaged resources should only get disposed if this method is executed for the first time.
        /// </summary>
        /// <param name="disposeManagedResources">Indicates whether managed resources should be disposed.</param>
        protected virtual void Dispose(bool disposeManagedResources)
        {
        }

        /// <summary>
        /// Called when a property is changed.
        /// </summary>
        /// <typeparam name="T">The type parameter.</typeparam>
        /// <param name="expression">The expression.</param>
        protected virtual void OnPropertyChanged<T>(Expression<Func<T, object>> expression)
        {
            this.OnPropertyChanged(GetPropertyName(expression));
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <typeparam name="T">The type parameter.</typeparam>
        /// <param name="expression">A lambda expression like 'n =&gt; n.PropertyName'.</param>
        /// <returns>
        /// The name of the property if property exists, otherwise <c>null</c>.
        /// </returns>
        private static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            var lambda = expression as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }

            Debug.Assert(memberExpression != null, "Please provide a lambda expression like 'n => n.PropertyName'");

            if (memberExpression != null)
            {
                var propertyInfo = memberExpression.Member as PropertyInfo;

                return propertyInfo.Name;
            }

            return null;
        }
    }
}