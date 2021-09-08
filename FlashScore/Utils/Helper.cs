using System;
using System.Runtime.CompilerServices;

namespace FlashScore.Utils
{
    internal static class Helper
    {
        /// <summary> Вызывает исключение ArgumentNullException, если значение равно null. </summary>
        /// <typeparam name="T"></typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T HasNull<T>(this T value, string parameterName = "") where T : class
        {
            ThrowIfNull(value, parameterName);
            return value;
        }

        /// <summary> Вызывает исключение ArgumentNullException, если значение равно null. </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameterName">Имя переменной которая проверяется</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNull<T>(this T value, string parameterName = "") where T : class
        {
            if (value is null) throw new ArgumentNullException(parameterName);
        }

        /// <summary> Вызывает исключение ArgumentException, если строка равна null или была пустой. </summary>
        /// <param name="str">Строка для проверки.</param>
        /// <returns>Строка после проверки.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string HasNullOrEmpty(string str)
        {
            ThrowIfNullOrEmpty(str);
            return str;
        }

        /// <summary> Вызывает исключение ArgumentException, если строка равна null или была пустой. </summary>
        /// <param name="str">Строка для проверки.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNullOrEmpty(string str)
        {
            if (string.IsNullOrEmpty(str)) throw new ArgumentException();
        }
    }
}
