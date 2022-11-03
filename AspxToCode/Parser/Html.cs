using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using HtmlAgilityPack;

namespace AspxToCode.Parser
{
    public static class Html
    {
        public static string CleanHtml(string source)
        {
            var config = Configuration.Default;

            using var context = BrowsingContext.New(config);
            var doc = GetDocument(context, source);

            return doc.ToHtml();
        }

        public static List<string> ParseHtml(string source)
        {
            var ret = new List<string>();

            /*
            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(source);

            var nodes = htmlDoc.DocumentNode.Descendants().ToList();

            foreach (var node in nodes)
            {
                
            }
            */

            var reader = new Sgml.SgmlReader();

            reader.InputStream = new StringReader(source);
            while (!reader.EOF)
            {
                reader.Read();
            }


            return ret;
        }

        private static IDocument GetDocument(IBrowsingContext context, string source)
        {
            var task = Task.Run(async () => await context.OpenAsync(req => req.Content(source)));
            var doc = task.Result;
            return doc;
        }
    }
}
