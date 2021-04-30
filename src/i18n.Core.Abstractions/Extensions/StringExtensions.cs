using System;
using System.Text;

namespace i18n.Core.Abstractions.Extensions
{
    internal static class StringExtensions
    {
        /// <summary>
        /// String extension method to simplify testing for non-null/non-empty values.
        /// </summary>
        public static bool IsSet(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }
    }
}
