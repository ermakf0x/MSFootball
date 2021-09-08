using System.ComponentModel;

namespace FS.Core
{
    /// <summary> Сторона за которую играет команда(игрок) <summary>
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Side : byte
    {
        First,
        Second
    }
}
