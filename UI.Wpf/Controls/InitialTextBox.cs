using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

namespace Palmmedia.BackUp.UI.Wpf.Controls
{
    /// <summary>
    /// A TextBox that can display a initial value
    /// </summary>
    public class InitialTextBox : TextBox
    {
        /// <summary>
        /// Dependency Property to access the InitialText.
        /// </summary>
        public static readonly DependencyProperty InitialTextProperty = DependencyProperty.Register(
            "InitialText",
            typeof(string),
            typeof(InitialTextBox),
            new PropertyMetadata(string.Empty));

        /// <summary>
        /// Determines whether a text is entered.
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess", Justification = "Must be placed before 'HasValueProperty', otherwise an runtime error occurs.")]
        private static readonly DependencyPropertyKey HasValuePropertyKey = DependencyProperty.RegisterReadOnly(
            "HasValue",
            typeof(bool),
            typeof(InitialTextBox),
            new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Determines whether a text is entered.
        /// </summary>
        public static readonly DependencyProperty HasValueProperty = HasValuePropertyKey.DependencyProperty;

        /// <summary>
        /// Initializes static members of the <see cref="InitialTextBox"/> class.
        /// </summary>
        static InitialTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InitialTextBox), new FrameworkPropertyMetadata(typeof(InitialTextBox)));
            TextProperty.OverrideMetadata(typeof(InitialTextBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(TextPropertyChanged)));
        }

        /// <summary>
        /// Gets or sets a text that is displayed if text is empty.
        /// </summary>
        public string InitialText
        {
            get
            {
                return (string)GetValue(InitialTextProperty);
            }

            set
            {
                SetValue(InitialTextProperty, value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether a text is entered.
        /// </summary>
        public bool HasValue
        {
            get
            {
                return (bool)GetValue(HasValueProperty);
            }
        }

        /// <summary>
        /// Resets the text to an empty string.
        /// </summary>
        public void Reset()
        {
            this.Text = null;
        }

        /// <summary>
        /// Executed when text changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void TextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            InitialTextBox itb = (InitialTextBox)sender;

            bool actuallyHasText = itb.Text.Length > 0;
            if (actuallyHasText != itb.HasValue)
            {
                itb.SetValue(HasValuePropertyKey, actuallyHasText);
            }
        }
    }
}
