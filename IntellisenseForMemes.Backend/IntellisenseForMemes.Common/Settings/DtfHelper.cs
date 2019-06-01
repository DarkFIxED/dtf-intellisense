
using System;

namespace IntellisenseForMemes.Common.Settings
{
    public static class DtfHelper
    {
        public static string MemeNameFromComment(string commentContent)
        {
            var start = commentContent.IndexOf("${", StringComparison.Ordinal);
            if (start < 0)
            {
                return string.Empty;
            }
            var end = commentContent.IndexOf("}", start, StringComparison.Ordinal);
            if (end < 0)
            {
                return string.Empty;
            }

            var memeName = commentContent.Substring(start + 2, end - start - 2);

            return memeName;
        }
    }
}
