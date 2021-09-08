using MSFootball.Converters;
using System.ComponentModel;

namespace MSFootball.Models.Analiz
{
    /// <summary>
    /// Меньше, больше или равно
    /// </summary>
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum LessMoreEqual
    {
        /// <summary>
        /// Больше
        /// </summary>
        [Description("Больше")]
        More,
        /// <summary>
        /// Меньше
        /// </summary>
        [Description("Меньше")]
        Less,
        /// <summary>
        /// Равно
        /// </summary>
        [Description("Равно")]
        Equal
    }
}
