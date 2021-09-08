using FlashScore.Utils;
using System.ComponentModel;

namespace FlashScore
{
    /// <summary> Сторона за которую играет команда(игрок) <summary>
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Side : byte
    {
        First,
        Second
    }
}
