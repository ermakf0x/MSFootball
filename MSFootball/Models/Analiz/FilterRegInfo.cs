using System;

namespace MSFootball.Models.Analiz
{
    /// <summary>
    /// Содержит информацию о фильтре, имя и тип
    /// </summary>
    struct FilterRegInfo
    {
        /// <summary>
        /// Имя фильтра
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Тип фильтра
        /// </summary>
        public Type Type { get; }

        public FilterRegInfo(string name, Type type)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }
    }
}
