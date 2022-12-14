using System.Diagnostics.CodeAnalysis;
using System.Xml;
using Sgml.Parser.Enum;

namespace Sgml;

/// <summary>
/// This class models an XML node, an array of elements in scope is maintained while parsing
/// for validation purposes, and these Node objects are reused to reduce object allocation,
/// hence the reset method.  
/// </summary>
internal class Node
{
    internal XmlNodeType NodeType;
    internal string Value;
    internal XmlSpace Space;
    internal string XmlLang;
    internal bool IsEmpty;
    internal string Name;
    internal ElementDecl DtdType; // the DTD type found via validation
    internal State CurrentState;
    internal bool Simulated; // tag was injected into result stream.
    HWStack attributes = new HWStack(10);

    /// <summary>
    /// Attribute objects are reused during parsing to reduce memory allocations, 
    /// hence the Reset method. 
    /// </summary>
    public void Reset(string name, XmlNodeType nt, string value)
    {
        Value = value;
        Name = name;
        NodeType = nt;
        Space = XmlSpace.None;
        XmlLang = null;
        IsEmpty = true;
        attributes.Count = 0;
        DtdType = null;
    }

    public Attribute AddAttribute(string name, string value, char quotechar, bool caseInsensitive)
    {
        Attribute a;
        // check for duplicates!
        for (int i = 0, n = attributes.Count; i < n; i++)
        {
            a = (Attribute)attributes[i];
            if (string.Equals(a.Name, name, caseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
            {
                return null;
            }
        }
        // This code makes use of the high water mark for attribute objects,
        // and reuses exisint Attribute objects to avoid memory allocation.
        a = (Attribute)attributes.Push();
        if (a == null)
        {
            a = new Attribute();
            attributes[attributes.Count - 1] = a;
        }
        a.Reset(name, value, quotechar);
        return a;
    }

    [SuppressMessage("Microsoft.Performance", "CA1811", Justification = "Kept for potential future usage.")]
    public void RemoveAttribute(string name)
    {
        for (int i = 0, n = attributes.Count; i < n; i++)
        {
            Attribute a = (Attribute)attributes[i];
            if (string.Equals(a.Name, name, StringComparison.OrdinalIgnoreCase))
            {
                attributes.RemoveAt(i);
                return;
            }
        }
    }
    public void CopyAttributes(Node n)
    {
        for (int i = 0, len = n.attributes.Count; i < len; i++)
        {
            Attribute a = (Attribute)n.attributes[i];
            Attribute na = AddAttribute(a.Name, a.Value, a.QuoteChar, false);
            na.DtdType = a.DtdType;
        }
    }

    public int AttributeCount
    {
        get
        {
            return attributes.Count;
        }
    }

    public int GetAttribute(string name)
    {
        for (int i = 0, n = attributes.Count; i < n; i++)
        {
            Attribute a = (Attribute)attributes[i];
            if (string.Equals(a.Name, name, StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }
        return -1;
    }

    public Attribute GetAttribute(int i)
    {
        if (i >= 0 && i < attributes.Count)
        {
            Attribute a = (Attribute)attributes[i];
            return a;
        }
        return null;
    }
}