using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MSFootball.Models.Analiz
{
    class ComboBoxEnumFilterProperty<T> : FilterPropertyBase<T> where T : Enum
    {
        ComboBox _comboBox;
        public override FrameworkElement ObjectView => _comboBox;

        public ComboBoxEnumFilterProperty()
        {
            _comboBox = new ComboBox();
            _comboBox.ItemsSource = Enum.GetValues(typeof(T));
            SetBinding(Selector.SelectedItemProperty);
            _comboBox.SelectedIndex = 0;
        }
    }
}
