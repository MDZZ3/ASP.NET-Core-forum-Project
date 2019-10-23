using Ganss.XSS;
using System;
using System.Collections.Generic;
using System.Text;

namespace firstWeb.Core.Helper
{
    public static class XSSHelper
    {
        private static HtmlSanitizer _Sanitizer;

         static XSSHelper()
        {
            _Sanitizer = new HtmlSanitizer();
        }

        public static string Sanitizer(string html)
        {
           return _Sanitizer.Sanitize(html);
        }
    }
}
