using ATA.Library.Shared.Service.Exceptions;
using DNTPersianUtils.Core;
using System;
using System.Globalization;

namespace ATA.Library.Shared.Service.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Check if string date is in a valid format. For Miladi dates, format should be like 2020/11/23 and for Shamsi it can be like 1398/02/22 or 1398-02-22 or 98/02/22 or 98-02-22
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static bool IsStringInValidDateFormat(this string strDate)
        {   // 2020/04/15 Or 1399/02/17 Are Valid Dates

            if (string.IsNullOrWhiteSpace(strDate))
                throw new ArgumentNullException(nameof(strDate));

            var cleanedStr = strDate.Trim().Fa2EnDigits();

            if (cleanedStr.StartsWith("20"))
            { // Miladi Date
                var formats = new[] { "yyyy/MM/dd" };
                if (!DateTime.TryParseExact(cleanedStr, formats, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out _))
                    return false;

                return true;
            }

            // Is Persian Date
            return cleanedStr.IsValidPersianDateTime();
        }

        /// <summary>
        /// Convert string date to DateTime type both for Persian and Georgian formats. Georgian date should be started with 20.
        /// </summary>
        /// <param name="strDate">string date both in Miladi or Shamsi format, usually taken from datepicker</param>
        /// <param name="strTime">string time using get from a picker or textbox. It is optional</param>
        /// <returns></returns>
        public static DateTime StringToDateTime(this string strDate, string? strTime = null)
        {
            if (string.IsNullOrWhiteSpace(strDate))
                throw new ArgumentNullException(nameof(strDate));

            if (!strTime.HasValue())
            {
                if (!strDate.IsStringInValidDateFormat()) throw new BadRequestException("فرمت تاریخ اشتباه است");

                var cleanedStr = strDate.Trim().Fa2EnDigits();

                if (cleanedStr.StartsWith("20")) // = Miladi Date
                    return Convert.ToDateTime(cleanedStr);

                return (DateTime)cleanedStr.ToGregorianDateTime()!;
            }

            var datePart = strDate.StringToDateTime();
            var timePart = DateTime.ParseExact(strTime, "HH:mm", null, DateTimeStyles.None);
            return new DateTime(datePart.Year, datePart.Month, datePart.Day,
                timePart.Hour, timePart.Minute, timePart.Second);
        }

        /// <summary>
        /// Compare dates with each other in a very easy way. Only dates (without time portion) will be compared in this method
        /// </summary>
        /// <param name="strLaterDate"></param>
        /// <param name="strSoonerDate"></param>
        /// <param name="includeEqualDate"> Same dates return true or false based on this param </param>
        /// <returns></returns>
        public static bool IsThisDateAfterThan(this string strLaterDate, string strSoonerDate, bool includeEqualDate = true)
        {
            if (includeEqualDate)
                return DateTime.Compare(strSoonerDate.StringToDateTime(), strLaterDate.StringToDateTime()) <= 0;

            return DateTime.Compare(strSoonerDate.StringToDateTime(), strLaterDate.StringToDateTime()) < 0;
        }

        /// <summary>
        /// Check if a string time is in valid format
        /// </summary>
        /// <param name="strTime"></param>
        /// <returns></returns>
        public static bool IsStringInValidTimeFormat(this string strTime)
        {
            if (string.IsNullOrWhiteSpace(strTime))
                throw new ArgumentNullException(nameof(strTime));

            var cleanedStr = strTime.Trim().Fa2EnDigits();
            return TimeSpan.TryParse(cleanedStr, out var dummyOutput);
        }

        /// <summary>
        /// Convert time part of a datetime type to total minutes
        /// </summary>
        /// <param name="strDateTime"></param>
        /// <returns></returns>
        public static int ConvertTimePartToMinutes(this string strDateTime)
        {
            return strDateTime.ConvertTimePartToSeconds() / 60;
        }

        public static int ConvertTimePartToSeconds(this string strDateTime)
        {
            if (string.IsNullOrWhiteSpace(strDateTime))
                throw new ArgumentNullException(nameof(strDateTime));

            var cleanedStr = strDateTime.Trim().Fa2EnDigits();
            var time = DateTime.Parse(cleanedStr, new CultureInfo("en-US", false));
            return (int)(time - time.Date).TotalSeconds;
        }
    }
}