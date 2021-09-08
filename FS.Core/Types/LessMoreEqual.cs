using System.ComponentModel;

namespace FS.Core
{
    /// <summary>
    /// Меньше, больше или равно
    /// </summary>
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum LessMoreEqual : byte
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
