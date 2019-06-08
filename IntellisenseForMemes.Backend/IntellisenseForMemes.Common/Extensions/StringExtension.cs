using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace IntellisenseForMemes.Common.Extensions
{
    public static class StringExtension
    {
        public static string NormalizeWords(this string input)
        {
            var lowerCaseInput = input.ToLower();

            var words = Regex.Matches(lowerCaseInput, "[a-z0-9а-я]+");

            return string.Join(" ", words);
        }
    }
}
