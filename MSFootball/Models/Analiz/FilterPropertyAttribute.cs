using System;

namespace MSFootball.Models.Analiz
{
    [AttributeUsage(AttributeTargets.Field)]
    class FilterPropertyAttribute : Attribute
    {
        public string PropertyName { get; set; }

        public FilterPropertyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
