using System;
using System.Runtime.CompilerServices;

namespace FS.Football.Utils
{
    static class Helper
    {
        /// <summary>
        /// Вызывает исключение ArgumentNullException, если значение равно null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T HasNull<T>(this T value, string parameterName = "") where T : class
        {
            IfNullThenThrow(value, parameterName);
            return value;
        }

        /// <summary>
        /// Вызывает исключение ArgumentNullException, если значение равно null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameterName">Имя переменной которая проверяется</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IfNullThenThrow<T>(this T value, string parameterName = "") where T : class
        {
            if (value is null) throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        /// Вызывает исключение ArgumentNullException или ArgumentException, если строка равна null или была пустой.
        /// </summary>
        /// <param name="str">Строка для проверки.</param>
        /// <returns>Строка после проверки.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string HasNullOrWhiteSpace(string str)
        {
            if (str is null) throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(str) || str == "") throw new ArgumentException();
            return str;
        }
    }
}
