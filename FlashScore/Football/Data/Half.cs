using FlashScore.Utils;
using System.ComponentModel;

namespace FlashScore.Football
{
    /// <summary> Игровой(Футбольный) тайм </summary>
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Half : byte
    {
        /// <summary> Весь матч </summary>
        [Description("Весь матч")]
        Match = 0,
        /// <summary> 1й тайм </summary>
        [Description("1й тайм")]
        First = 1,
        /// <summary> 2й тайм </summary>
        [Description("2й тайм")]
        Second = 2
    }
}
