using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace MSFootball.Models.Analiz
{
    class FilterProperties
    {
        readonly FilterBase filter;
        List<FrameworkElement> _properties;
        public IReadOnlyList<FrameworkElement> Properties => _properties;
        public FilterProperties(FilterBase filter)
        {
            this.filter = filter ?? throw new ArgumentNullException(nameof(filter));
            _properties = new List<FrameworkElement>();
        }

        public void Build()
        {
            var props = GetProperties();
            if (props.Count == 0) return;

            var group = new Dictionary<string, List<FPInfo>>();

            foreach (var p in props)
            {
                if (!group.ContainsKey(p.PropertyName))
                    group.Add(p.PropertyName, new List<FPInfo>());
                group[p.PropertyName].Add(p);
            }

            foreach (var g in group)
            {
                if (g.Value.Count == 1)
                {
                    _properties.Add(CreateGroupBox(g.Key, g.Value[0].Property));
                }
                else
                {
                    var sp = new StackPanel();
                    for (int i = 0; i < g.Value.Count; i++)
                    {
                        var element = g.Value[i].Property;
                        if (i < g.Value.Count - 1)
                            element.Margin = new Thickness(0, 0, 0, 5);
                        sp.Children.Add(element);
                    }
                    _properties.Add(CreateGroupBox(g.Key, sp));
                }
            }
        }

        List<FPInfo> GetProperties()
        {
            var res = new List<FPInfo>();
            var fType = filter.GetType();

            foreach (var p in fType.GetRuntimeFields())
            {
                foreach (var att in p.GetCustomAttributes())
                {
                    if (att is FilterPropertyAttribute att2 && p.GetValue(filter) is IFilterProperty fp)
                    {
                        var fpinfo = new FPInfo
                        {
                            Property = fp.ObjectView,
                            PropertyName = att2.PropertyName
                        };

                        res.Add(fpinfo);
                        break;
                    }
                }
            }

            return res;
        }
        GroupBox CreateGroupBox(string header, FrameworkElement content)
        {
            var _gb = new GroupBox();
            _gb.Padding = new Thickness(2, 3, 2, 3);
            _gb.Header = header;
            _gb.Content = content;
            return _gb;
        }

        struct FPInfo
        {
            public FrameworkElement Property { get; set; }
            public string PropertyName { get; set; }

            public override string ToString()
            {
                return PropertyName;
            }
        }
    }
}
