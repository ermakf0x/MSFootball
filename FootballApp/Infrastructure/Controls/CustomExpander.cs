using System.Windows;
using System.Windows.Controls;

namespace FootballApp.Infrastructure.Controls
{
    class CustomExpander : Expander
    {
        public bool HeaderMouseOver
        {
            get { return (bool)GetValue(HeaderMouseOverProperty); }
            set { SetValue(HeaderMouseOverProperty, value); }
        }
        public static readonly DependencyProperty HeaderMouseOverProperty =
            DependencyProperty.Register("HeaderMouseOver", typeof(bool), typeof(CustomExpander), new PropertyMetadata(false));
    }
}
