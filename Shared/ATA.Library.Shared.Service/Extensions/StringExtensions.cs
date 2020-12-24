using System;

namespace ATA.Library.Shared.Service.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        ///Get String Between Two String In a String
        /// </summary>      
        public static string Between(this string value, string str1, string str2)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            var str1BeginIndex = value.IndexOf(str1, StringComparison.Ordinal);
            if (str1BeginIndex == -1)
                return "";
            var str2BeginIndex = value.IndexOf(str2, str1BeginIndex, StringComparison.Ordinal);
            if (str2BeginIndex == -1)
                return "";
            var str1EndIndex = str1BeginIndex + str1.Length;
            return str1EndIndex >= str2BeginIndex ? ""
                : value.Substring(str1EndIndex, str2BeginIndex - str1EndIndex);
        }

        public static string After(this string value, string str)
        {
            var strBeginIndex = value.LastIndexOf(str, StringComparison.Ordinal);
            if (strBeginIndex == -1)
                return "";
            var strEndIndex = strBeginIndex + str.Length;
            return strBeginIndex >= value.Length ? "" : value.Substring(strEndIndex);
        }

        public static bool HasValue(this string? value, bool ignoreWhiteSpace = true)
        {
            return ignoreWhiteSpace ? !string.IsNullOrWhiteSpace(value) : !string.IsNullOrEmpty(value);
        }

        public static int ToInt(this string value)
        {
            return Convert.ToInt32(value.Trim().Fa2EnDigits());
        }

        public static long ToLong(this string value)
        {
            return Convert.ToInt64(value.Trim().Fa2EnDigits());
        }

        public static decimal ToDecimal(this string value)
        {
            return Convert.ToDecimal(value.Trim().Fa2EnDigits());
        }

        public static string ToNumeric(this int value)
        {
            return value.ToString("N0"); //"123,456"
        }

        public static string ToNumeric(this decimal value)
        {
            return value.ToString("N0");
        }

        public static string ToCurrency(this int value)
        {
            //fa-IR => current culture currency symbol => ریال
            //123456 => "123,123ریال"
            return value.ToString("C0");
        }

        public static string ToCurrency(this decimal value)
        {
            return value.ToString("C0");
        }

        public static string En2FaDigits(this string str)
        {
            return str.Replace("0", "۰")
                .Replace("1", "۱")
                .Replace("2", "۲")
                .Replace("3", "۳")
                .Replace("4", "۴")
                .Replace("5", "۵")
                .Replace("6", "۶")
                .Replace("7", "۷")
                .Replace("8", "۸")
                .Replace("9", "۹");
        }

        public static string Fa2EnDigits(this string str)
        {
            return str.Replace("۰", "0")
                .Replace("۱", "1")
                .Replace("۲", "2")
                .Replace("۳", "3")
                .Replace("۴", "4")
                .Replace("۵", "5")
                .Replace("۶", "6")
                .Replace("۷", "7")
                .Replace("۸", "8")
                .Replace("۹", "9")
                //iphone numeric
                .Replace("٠", "0")
                .Replace("١", "1")
                .Replace("٢", "2")
                .Replace("٣", "3")
                .Replace("٤", "4")
                .Replace("٥", "5")
                .Replace("٦", "6")
                .Replace("٧", "7")
                .Replace("٨", "8")
                .Replace("٩", "9");
        }

        public static string FixPersianChars(this string str)
        {
            return str.Replace("ﮎ", "ک")
                .Replace("ﮏ", "ک")
                .Replace("ﮐ", "ک")
                .Replace("ﮑ", "ک")
                .Replace("ك", "ک")
                .Replace("ي", "ی")
                .Replace(" ", " ")
                .Replace("‌", " ")
                .Replace("ھ", "ه");//.Replace("ئ", "ی");
        }

        public static string? CleanString(this string str)
        {
            return str.Trim().FixPersianChars().Fa2EnDigits().NullIfEmpty();
        }

        public static string? NullIfEmpty(this string str)
        {
            return str?.Length == 0 ? null : str;
        }

        public static bool IsConvertableToInt(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentNullException(nameof(str));

            var cleanedStr = str.Trim().Fa2EnDigits();
            return int.TryParse(cleanedStr, out _);
        }

        public static bool IsConvertableToDecimal(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentNullException(nameof(str));

            var cleanedStr = str.Trim().Fa2EnDigits();
            return decimal.TryParse(cleanedStr, out _);
        }


    }
}