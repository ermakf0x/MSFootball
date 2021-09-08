using System;
using System.Collections.Generic;

namespace MSFootball.Models.Analiz
{
    /// <summary>
    /// Менеджер фильтров
    /// </summary>
    internal static class FilterManager
    {
        static List<FilterRegInfo> _filters;
        /// <summary>
        /// Список зарегестрированных фильтров
        /// </summary>
        public static IReadOnlyList<FilterRegInfo> Filters { get; }

        static FilterManager()
        {
            _filters = new List<FilterRegInfo>();
            Filters = _filters;

            #region Добавляем базовые фильтры

            Register<FTotal>("Тотал");
            Register<FTotalMissed>("Тотал пропущенных");
            Register<FTotalClogged>("Тотал забитых");
            Register<FVictory>("Победы");
            Register<FDefeat>("Поражения");
            Register<FDraw>("Ничьи");
            Register<FBothWillScore>("Обе забьют");
            Register<FStatistics>("Статистика");
            Register<FGoal>("Гол");

            #endregion
        }
        /// <summary>
        /// Регистрирует новый фильтер
        /// </summary>
        /// <typeparam name="T">Тип фильтра</typeparam>
        /// <param name="name">Имя фильтра</param>
        public static void Register<T>(string name) where T : FilterBase
        {
            _filters.Add(new FilterRegInfo(name, typeof(T)));
        }
        /// <summary>
        /// Создает экземпляр нового фильтра
        /// </summary>
        /// <param name="filterRegInfo">Информация о регистрированном фильтре</param>
        /// <returns>Созданный фильтер</returns>
        public static FilterBase CreateInstance(FilterRegInfo filterRegInfo)
        {
            var filter = Activator.CreateInstance(filterRegInfo.Type) as FilterBase;
            filter.Name = filterRegInfo.Name;
            return filter;
        }
    }
}
