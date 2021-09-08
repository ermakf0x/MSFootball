namespace MSFootball.Models
{
    /// <summary>
    ///  Вспомагательный класс с глобальными переменными
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// Лимит на загрузку сыгранных матчей
        /// </summary>
        public static ushort Limit { get; set; } = 3;
    }
}
