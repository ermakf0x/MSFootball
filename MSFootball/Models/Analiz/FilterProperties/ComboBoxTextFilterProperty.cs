using System.Windows;
using System.Windows.Controls;

namespace MSFootball.Models.Analiz
{
    class ComboBoxTextFilterProperty : FilterPropertyBase<string>
    {
        ComboBox _comboBox;
        public override FrameworkElement ObjectView => _comboBox;

        public ComboBoxTextFilterProperty(params string[] items)
        {
            _comboBox = new ComboBox();
            _comboBox.ItemsSource = items;
            SetBinding(System.Windows.Controls.Primitives.Selector.SelectedItemProperty);
            _comboBox.SelectedIndex = 0;
        }
    }
}
