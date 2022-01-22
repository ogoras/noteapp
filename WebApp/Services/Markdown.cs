using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Westwind.AspNetCore.Markdown;
using Westwind.Web.Markdown.MarkdownParser;

namespace WebApp.Services
{
    public static class Markdown
    {
        public static string Parse(string markdown,
                        bool usePragmaLines = false,
                        bool forceReload = false)
        {
            if (string.IsNullOrEmpty(markdown))
                return "";

            var parser = MarkdownParserFactory.GetParser(usePragmaLines, forceReload);
            return parser.Parse(markdown);
        }

        public static HtmlString ParseHtmlString(string markdown,
                        bool usePragmaLines = false,
                        bool forceReload = false)
        {
            return new HtmlString(Parse(markdown, usePragmaLines, forceReload));
        }
    }
}
