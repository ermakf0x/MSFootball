using FlashScore.Utils;
using System.ComponentModel;

namespace FlashScore
{
    /// <summary> Меньше, больше или равно </summary>
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum LessMoreEqual : byte
    {
        /// <summary> Больше </summary>
        [Description("Больше")]
        More,
        /// <summary> Меньше </summary>
        [Description("Меньше")]
        Less,
        /// <summary> Равно </summary>
        [Description("Равно")]
        Equal
    }
}
