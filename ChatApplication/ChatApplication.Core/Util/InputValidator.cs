using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ChatApplication.Core.Util
{
    public static class InputValidator
    {
        public const int PasswordMinimumCharsNum = 6;

        private static readonly HashSet<char> specialCharacters = new HashSet<char>() { '%', '$', '#' };
        private static readonly Regex usernameVerficationRegex = new Regex(@"[^\s,<,>,#,',\-,\/,\\;,!,,$,@]*");
        private static readonly Regex emailVerificationRegex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", RegexOptions.Compiled);
        private static readonly Regex passwordAllowedCharsRegex = new Regex(@"[^\s]*", RegexOptions.Compiled);
        private static readonly Regex httpUrlVerificationRegex = new Regex(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$");

        public static bool IsValidPassword(string password) =>
            password.Length >= PasswordMinimumCharsNum
            && IsValidFormat(password, passwordAllowedCharsRegex)
            && password.Any(char.IsLower)
            && password.Any(char.IsUpper)
            && password.Any(char.IsDigit)
            && password.Any(specialCharacters.Contains);

        public static bool IsValidEmail(string email) => IsValidFormat(email, emailVerificationRegex);

        public static bool IsValidUsername(string username) => IsValidFormat(username, usernameVerficationRegex);

        public static bool IsValidHttpUrl(string url) => IsValidFormat(url, httpUrlVerificationRegex);

        public static bool IsValidString(string value) => !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value);

        private static bool IsValidFormat(string forValidation, Regex regex)
            => RegexUtil.IsMatch(forValidation, regex);
    }
}
