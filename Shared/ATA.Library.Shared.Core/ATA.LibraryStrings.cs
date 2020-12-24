namespace ATA.Library.Shared.Core
{
    public static class AppStrings
    {
        public static readonly string ATASecurityAppKeyName = "library";

        public static readonly string AppPersianFullName = "سامانه‌ی کتابخانه‌ی مجازی آتا";

        public static readonly string AppEnglishFullName = "ATA Virtual Library";

        public static readonly string ATAAuthTokenKey = "SSOToken";

        /// <summary>
        /// Strings that only apply to web app
        /// </summary>
        public static class WebApp
        {
            public static readonly string ClientName = "LibraryBlazorApp";
            public static readonly string ClientId = "LibraryBlazorApp-ResOwnerFlow";
        }

        public class Claims
        {
            public static readonly string Administrator = nameof(Administrator);

            public static readonly string PersonnelCode = nameof(PersonnelCode);

            public static readonly string Username = nameof(Username);
        }

        public static class Regex
        {
            public const string EmailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

            // See <see href="http://stackoverflow.com/a/7933253/433790" /> to find regex source.
            public const string SubDomainRegex = "[A-Za-z0-9](?:[A-Za-z0-9\\-]{0,61}[A-Za-z0-9])?";

            // See <see href="https://stackoverflow.com/a/17773849/6661314" /> to find regex source.
            public const string UrlRegex = @"(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})";

            // See <see href="https://stackoverflow.com/questions/1636350/how-to-identify-a-given-string-is-hex-color-format" /> to find regex source.
            public const string HexColor = "^#(?:[0-9a-fA-F]{3}){1,2}$";

            // See <see href="https://stackoverflow.com/questions/19605150/regex-for-password-must-contain-at-least-eight-characters-at-least-one-number-a"/> to find regex source.
            public static string Password = "^(?=.*[\\w])(?=.*[\\W])[\\w\\W]{8,}$";

            // See <see href="https://www.regextester.com/93648"/> to find regex source.
            public static string Name = "^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$";
        }

    }
}