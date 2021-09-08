using System.Windows;
using System.Windows.Data;

namespace MSFootball.Models.Analiz
{
    abstract class FilterPropertyBase<T> : IFilterProperty
    {
        public T Value { get; set; }
        public abstract FrameworkElement ObjectView { get; }

        protected void SetBinding(DependencyProperty dp)
        {
            ObjectView.SetBinding(dp, new Binding
            {
                Source = this,
                Path = new PropertyPath("Value")
            });
        }
    }
}
