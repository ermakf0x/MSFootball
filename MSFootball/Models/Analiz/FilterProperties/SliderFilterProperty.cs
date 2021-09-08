using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MSFootball.Models.Analiz
{
    class SliderFilterProperty : FilterPropertyBase<double>
    {
        DockPanel _dockPanel;
        public override FrameworkElement ObjectView => _dockPanel;
        public SliderFilterProperty(double minimum, double maximum, double tickFrequency, int autoToolTipPrecision = 0)
        {
            var _slider = new Slider
            {
                Minimum = minimum,
                Maximum = maximum,
                TickFrequency = tickFrequency,
                IsSnapToTickEnabled = true,
                AutoToolTipPlacement = System.Windows.Controls.Primitives.AutoToolTipPlacement.TopLeft,
                AutoToolTipPrecision = autoToolTipPrecision
            };
            _slider.SetBinding(System.Windows.Controls.Primitives.RangeBase.ValueProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath("Value")
            });

            var _textBlock = new TextBlock
            {
                TextAlignment = TextAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 30
            };
            _textBlock.SetBinding(TextBlock.TextProperty, new Binding
            {
                Source = _slider,
                Path = new PropertyPath("Value"),
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

            DockPanel.SetDock(_textBlock, Dock.Right);
            _dockPanel = new DockPanel();
            _dockPanel.Children.Add(_textBlock);
            _dockPanel.Children.Add(_slider);
        }
    }
}
