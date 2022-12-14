using System.Globalization;

namespace Sgml;

internal class Ucs4DecoderLittleEndian : Ucs4Decoder
{
    internal override int GetFullChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
        uint code;
        int i, j;
        byteCount += byteIndex;
        for (i = byteIndex, j = charIndex; i + 3 < byteCount;)
        {
            code = (uint)(bytes[i] << 24 | bytes[i + 1] << 16 | bytes[i + 2] << 8 | bytes[i + 3]);
            if (code > 0x10FFFF)
            {
                throw new SgmlParseException(string.Format(CultureInfo.CurrentUICulture, "Invalid character 0x{0:x} in encoding", code));
            }
            else if (code > 0xFFFF)
            {
                chars[j] = UnicodeToUTF16(code);
                j++;
            }
            else
            {
                if (code >= 0xD800 && code <= 0xDFFF)
                {
                    throw new SgmlParseException(string.Format(CultureInfo.CurrentUICulture, "Invalid character 0x{0:x} in encoding", code));
                }
                else
                {
                    chars[j] = (char)code;
                }
            }
            j++;
            i += 4;
        }
        return j - charIndex;
    }
}