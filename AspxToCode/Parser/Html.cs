using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;

namespace AspxToCode.Parser
{
    public static class Html
    {
        public static List<string> GetCode(string source)
        {
            List<string> ret = new();

            var config = Configuration.Default;

            using var context = BrowsingContext.New(config);
            var doc = GetDocument(context, source);

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
