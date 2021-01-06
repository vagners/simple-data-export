using System.Text.RegularExpressions;

namespace System
{
    public static class StringExtension
    {
        public static string Truncate(this string value, int length)
        {
            if (length > value.Length)
                return value;

            return value.Substring(0, length);
        }

        public static string OnlyNumbers(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            return Regex.Replace(value, "[^0-9]", string.Empty);
        }
    }
}