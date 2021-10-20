using System;
using System.Text.RegularExpressions;

namespace ChatApplication.Core.Util
{
    public class RegexUtil
    {
        public static bool IsMatch(string forValidation, Regex regex)
        {
            try
            {
                if (string.IsNullOrEmpty(forValidation) || string.IsNullOrWhiteSpace(forValidation))
                    return false;

                return regex.IsMatch(forValidation);
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }
    }
}
