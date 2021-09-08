using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Xceed.Wpf.Toolkit;

namespace MSFootball.Models.Analiz
{
    class RangeSliderFilterProperty : FilterPropertyBase<double>
    {
        DockPanel _dockPanel;
        public override FrameworkElement ObjectView => _dockPanel;
        public double Value2 { get; set; }

        public RangeSliderFilterProperty(double minimum, double maximum, double tickFrequency)
        {
            var _rangeSlider = new RangeSlider();
            _rangeSlider.Minimum = minimum;
            _rangeSlider.Maximum = maximum;
            _rangeSlider.TickFrequency = tickFrequency;
            _rangeSlider.IsSnapToTickEnabled = true;
            _rangeSlider.SetBinding(RangeSlider.LowerValueProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath("Value")
            });
            _rangeSlider.SetBinding(RangeSlider.HigherValueProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath("Value2")
            });
            _rangeSlider.HigherValue = maximum;

            var _textBlock = new TextBlock
            {
                TextAlignment = TextAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 30
            };
            _textBlock.SetBinding(TextBlock.TextProperty, new Binding
            {
                Source = _rangeSlider,
                Path = new PropertyPath("LowerValue"),
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            
            var _textBlock2 = new TextBlock
            {
                TextAlignment = TextAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 30
            };
            _textBlock2.SetBinding(TextBlock.TextProperty, new Binding
            {
                Source = _rangeSlider,
                Path = new PropertyPath("HigherValue"),
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

            DockPanel.SetDock(_textBlock, Dock.Left);
            DockPanel.SetDock(_textBlock2, Dock.Right);
            _dockPanel = new DockPanel();
            _dockPanel.Children.Add(_textBlock);
            _dockPanel.Children.Add(_textBlock2);
            _dockPanel.Children.Add(_rangeSlider);
        }
    }
}
