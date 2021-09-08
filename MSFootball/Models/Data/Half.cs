using MSFootball.Converters;
using System.ComponentModel;

namespace MSFootball.Models.Data
{
    /// <summary>
    /// Игровой(Футбольный) тайм
    /// </summary>
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Half
    {
        /// <summary>
        /// Весь матч
        /// </summary>
        [Description("Весь матч")]
        Match,
        /// <summary>
        /// 1й тайм
        /// </summary>
        [Description("1й тайм")]
        First,
        /// <summary>
        /// 2й тайм
        /// </summary>
        [Description("2й тайм")]
        Second
    }
}
