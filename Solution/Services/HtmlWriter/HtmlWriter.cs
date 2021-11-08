using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.HtmlWriter
{
    public static class HtmlWriter
    {
        public static string A(string link, string content)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("<a href='").Append(link).Append("'>");
            stringBuilder.Append(content);
            stringBuilder.Append("</a>");

            return stringBuilder.ToString();
        }

        public static string Br()
        {
            return "<br/>";
        }
    }
}
