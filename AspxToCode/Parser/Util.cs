using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using AspxToCode.Parser.Xml;

namespace AspxToCode.Parser
{
    public static class Util
    {
        public static List<string> HtmlToCode(string? source)
        {
            var result = new List<string>();

            var outputFile = new StringWriter();

            if (source == null)
                return result;

            FilteringReader reader = new(source);

            var writer = new FilteringWriter(outputFile)
            {
                FilterOutput = false,
                ConvertPrefixesToTags = true
            };

            reader.Read();
            while (!reader.EOF)
            {
            }

            return result;
        }

        public static List<string> GetNodes(string? source)
        {
            var result = new List<string>();

            if (source == null)
                return result;

            FilteringReader reader = new(source);

            reader.MoveToContent();
            // Parse the file and display each of the nodes.
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        result.Add($"<{reader.Name}>");
                        break;
                    case XmlNodeType.Text:
                        result.Add(reader.Value);
                        break;
                    case XmlNodeType.CDATA:
                        result.Add($"<![CDATA[{reader.Value}]]>");
                        break;
                    case XmlNodeType.ProcessingInstruction:
                        result.Add($"<?{reader.Name} {reader.Value}?>");
                        break;
                    case XmlNodeType.Comment:
                        result.Add($"<!--{reader.Value}-->");
                        break;
                    case XmlNodeType.XmlDeclaration:
                        result.Add("<?xml version='1.0'?>");
                        break;
                    case XmlNodeType.Document:
                        break;
                    case XmlNodeType.DocumentType:
                        result.Add($"<!DOCTYPE {reader.Name} [{reader.Value}]");
                        break;
                    case XmlNodeType.EntityReference:
                        result.Add(reader.Name);
                        break;
                    case XmlNodeType.EndElement:
                        result.Add("</{reader.Name}>");
                        break;
                }
            }

            return result;
        }

    }
}
