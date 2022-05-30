using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Haystack.String;

/// <summary>
/// Extensions for System.String
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Removes the specified string from the trailing end of the input string, if there's a match
    /// </summary>
    /// <param name="input"></param>
    /// <param name="suffixToRemove"></param>
    /// <param name="comparisonType"></param>
    /// <returns>The string with the specified suffix trimmed</returns>
    public static string TrimEnd(this string input, string suffixToRemove, StringComparison comparisonType)
    {
        if (suffixToRemove == null)
        {
            return input;
        }

        return input?.EndsWith(suffixToRemove, comparisonType) ?? false
            ? input.Substring(0, input.Length - suffixToRemove.Length)
            : input;
    }

    /// <summary>
    /// <inheritdoc cref="TrimEnd(string,string,System.StringComparison)"/> using StringComparison.CurrentCulture
    /// </summary>
    /// <param name="input"></param>
    /// <param name="suffixToRemove"></param>
    /// <returns></returns>
    public static string TrimEnd(this string input, string suffixToRemove)
        => TrimEnd(input, suffixToRemove, StringComparison.Ordinal);
        
    /// <summary>
    /// Removes the specified string from the beginning of the input string, if there's a match
    /// </summary>
    /// <param name="input"></param>
    /// <param name="prefixToRemove"></param>
    /// <param name="comparisonType"></param>
    /// <returns>The string with the specified prefix trimmed</returns>
    public static string TrimStart(this string input, string prefixToRemove, StringComparison comparisonType)
    {
        if (prefixToRemove == null)
        {
            return input;
        }

        return input?.StartsWith(prefixToRemove, comparisonType) ?? false
            ? input.Substring(prefixToRemove.Length, input.Length - prefixToRemove.Length)
            : input;
    }

    /// <summary>
    /// <inheritdoc cref="TrimStart(string,string,System.StringComparison)"/> using StringComparison.CurrentCulture
    /// </summary>
    /// <param name="input"></param>
    /// <param name="prefixToRemove"></param>
    /// <returns></returns>
    public static string TrimStart(this string input, string prefixToRemove)
        => TrimEnd(input, prefixToRemove, StringComparison.Ordinal);

    /// <summary>
    /// Removes the specified string from both the beginning and end of the input string, if there's a match
    /// </summary>
    /// <param name="input"></param>
    /// <param name="prefixAndSuffixPrefixToRemove"></param>
    /// <param name="comparisonType"></param>
    /// <returns>The string with the specified string removed from both ends</returns>
    public static string Trim(this string input, string prefixAndSuffixPrefixToRemove, StringComparison comparisonType)
        => input.TrimStart(prefixAndSuffixPrefixToRemove, comparisonType).TrimEnd(prefixAndSuffixPrefixToRemove, comparisonType);

    /// <summary>
    /// <inheritdoc cref="TrimEnd(string,string,System.StringComparison)"/> using StringComparison.CurrentCulture
    /// </summary>
    /// <param name="input"></param>
    /// <param name="prefixAndSuffixToRemove"></param>
    /// <returns></returns>
    public static string Trim(this string input, string prefixAndSuffixToRemove)
        => Trim(input, prefixAndSuffixToRemove, StringComparison.Ordinal);

    /// <summary>
    /// A search implementation that returns true if the input contains the search string as a substring using the specified comparison.
    /// </summary>
    /// <param name="haystack"></param>
    /// <param name="needle"></param>
    /// <param name="comparisonType"></param>
    /// <returns></returns>
    public static bool Contains(this string haystack, string needle, StringComparison comparisonType)
        => haystack.IndexOf(needle, comparisonType) >= 0;

    /// <summary>
    /// Hashes a string, and returns the first 16 bytes as a GUID which can be used where GUIDs and/or UNIQUEIDENTIFIERS are required.
    ///
    /// This is not a type 1-4 UUID in the technical sense. It merely creates stable bytes that will fit into a 128-bit UUID data structure, which can be
    /// useful in circumstances where you need stable, opaque identifiers that were once user-specified data.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static Guid ToStableGuid(this string str)
    {
        var asBytes = Encoding.UTF8.GetBytes(str);

        using (var hasher = SHA1.Create())
        {
            var hashedBytes = hasher.ComputeHash(asBytes)
                .Take(16)
                .ToArray();

            return new Guid(hashedBytes);
        }
    }

    /// <summary>
    /// A string comparison method safe from timing attacks
    /// </summary>
    /// <param name="str"></param>
    /// <param name="comparison"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static bool ConstantTimeEquals(this string str, string comparison, Encoding encoding)
    {
        if (str == null && comparison != null || str != null && comparison == null)
        {
            return false;
        }

        if (ReferenceEquals(str, comparison))
        {
            return true;
        }

        if (str.Length != comparison.Length)
        {
            return false;
        }

        using (var hasher = MD5.Create())
        {
            var leftBytes = hasher.ComputeHash(encoding.GetBytes(str));
            var rightBytes = hasher.ComputeHash(encoding.GetBytes(comparison));

            // This traditional XOR version is about 30ns faster than a naive `if`-based implementation
            var result = 0;
            for (var i = 0; i < str.Length; i++)
            {
                result |= str[i] ^ comparison[i];
            }
                
            return result == 0;
        }
    }

    /// <summary>
    /// <inheritdoc cref="ConstantTimeEquals(string,string,System.Text.Encoding)"/> using UTF-8 encoding
    /// </summary>
    /// <param name="str"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    public static bool ConstantTimeEquals(this string str, string comparison)
        => ConstantTimeEquals(str, comparison, Encoding.UTF8);
        
    /// <summary>
    /// Detects the encoding for UTF-7, UTF-8/16/32 (BOM, no BOM, big and little endian), and local default codepage, and potentially other codepages.
    /// </summary>
    /// <param name="asBytes">The bytes to taste and convert to string</param>
    /// <param name="tasteDepth">If UTF-8 is expected, use a large value, because characters that occur outside the ASCII range may not occur until late
    /// in the string</param>
    /// <returns>The Encoding and string associated with the byte array</returns>
    /// <exception cref="ArgumentException">In the event that an encoding cannot be discovered</exception>
    public static (Encoding encoding, string text) DetectEncoding(byte[] asBytes, int tasteDepth)
    {
        // From: https://stackoverflow.com/a/12853721/689185 by Dan W ( https://stackoverflow.com/users/848344/dan-w )

        // First check the low hanging fruit by checking if a BOM or signature exists
        // (sourced from http://www.unicode.org/faq/utf_bom.html#bom4 )
        if (asBytes.Length >= 4 && asBytes[0] == 0x00 && asBytes[1] == 0x00 && asBytes[2] == 0xFE && asBytes[3] == 0xFF)
        {
            var encoding = Encoding.GetEncoding("utf-32BE");
            var text = encoding.GetString(asBytes, 4, asBytes.Length - 4);
            return (encoding, text);
        }

        if (asBytes.Length >= 4 && asBytes[0] == 0xFF && asBytes[1] == 0xFE && asBytes[2] == 0x00 && asBytes[3] == 0x00)
        {
            var encoding = Encoding.UTF32;
            var text = encoding.GetString(asBytes, 4, asBytes.Length - 4);
            return (encoding, text);
        }

        if (asBytes.Length >= 2 && asBytes[0] == 0xFE && asBytes[1] == 0xFF)
        {
            var encoding = Encoding.BigEndianUnicode;
            var text = encoding.GetString(asBytes, 2, asBytes.Length - 2);
            return (encoding, text);
        }

        if (asBytes.Length >= 2 && asBytes[0] == 0xFF && asBytes[1] == 0xFE)
        {
            // UTF-16, little-endian
            var encoding = Encoding.Unicode;
            var text = encoding.GetString(asBytes, 2, asBytes.Length - 2);
            return (encoding, text);
        }

        if (asBytes.Length >= 3 && asBytes[0] == 0xEF && asBytes[1] == 0xBB && asBytes[2] == 0xBF)
        {
            var encoding = Encoding.UTF8;
            var text = Encoding.UTF8.GetString(asBytes, 3, asBytes.Length - 3);
            return (encoding, text);
        }

        if (asBytes.Length >= 3 && asBytes[0] == 0x2b && asBytes[1] == 0x2f && asBytes[2] == 0x76)
        {
            var encoding = Encoding.UTF7;
            var text = encoding.GetString(asBytes, 3, asBytes.Length - 3);
            return (encoding, text);
        }

        // At this point, we haven't found a BOM/signature, so now we'll "taste" the file to see if we can discover the encoding.
        // If we're dealing with UTF-8, we'll want a high taste depth, because characters outside the ASCII range may occur further
        // along in the string.
        if (tasteDepth == 0 || tasteDepth > asBytes.Length)
        {
            tasteDepth = asBytes.Length; // Taster size can't be bigger than the filesize obviously.
        }

        // Some text files encoded in UTF8 have no BOM/signature so we'll manually check for a UTF8 pattern.
        // This code is based off the top answer at:
        // https://stackoverflow.com/a/6555104/689185
        // For our purposes, an unnecessarily strict (and terser and slower) implementation can be found here:
        // https://stackoverflow.com/a/1031773/689185
        // For the below, false positives should be exceedingly rare, and would be either slightly malformed UTF-8
        // (which would suit our purposes anyway) or 8-bit extended ASCII/UTF-16/32 as a very long shot.
        var i = 0;
        var utf8 = false;
        while (i < tasteDepth - 4)
        {
            if (asBytes[i] <= 0x7F)
            {
                // If all characters are below 0x80, then it is valid UTF8, but UTF8 is not 'required' (and therefore the text is more desirable to be
                // treated as the default codepage of the computer).
                // This is why there's no utf8 = true -- unlike the next three checks.

                i += 1;
                continue;
            }

            if (asBytes[i] >= 0xC2
                && asBytes[i] <= 0xDF
                && asBytes[i + 1] >= 0x80
                && asBytes[i + 1] < 0xC0)
            {
                i += 2;
                utf8 = true;
                continue;
            }

            if (asBytes[i] >= 0xE0
                && asBytes[i] <= 0xF0
                && asBytes[i + 1] >= 0x80
                && asBytes[i + 1] < 0xC0
                && asBytes[i + 2] >= 0x80
                && asBytes[i + 2] < 0xC0)
            {
                i += 3;
                utf8 = true;
                continue;
            }

            if (asBytes[i] >= 0xF0
                && asBytes[i] <= 0xF4
                && asBytes[i + 1] >= 0x80
                && asBytes[i + 1] < 0xC0
                && asBytes[i + 2] >= 0x80
                && asBytes[i + 2] < 0xC0
                && asBytes[i + 3] >= 0x80
                && asBytes[i + 3] < 0xC0)
            {
                i += 4;
                utf8 = true;
                continue;
            }

            utf8 = false;
            break;
        }

        if (utf8)
        {
            var encoding = Encoding.UTF8;;
            var text = encoding.GetString(asBytes);
            return (encoding, text);
        }

        // The next check is a heuristic attempt to detect UTF-16 without a BOM: look for zeroes in odd or even byte places, and if a certain
        // threshold is reached, the code is 'probably' UTF-16.
        const double proportionOfMostSignificantBytesThatMustBeZeroToQualifyAsUtf16 = 0.1; //I.e. 10%
        var count = 0;
        for (var n = 0; n < tasteDepth; n += 2)
        {
            if (asBytes[n] == 0)
            {
                count++;
            }
        }

        var proportion = (double) count / tasteDepth;
        if (proportion > proportionOfMostSignificantBytesThatMustBeZeroToQualifyAsUtf16)
        {
            var encoding = Encoding.BigEndianUnicode;
            var text = encoding.GetString(asBytes);
            return (encoding, text);
        }

        count = 0;
        for (var n = 1; n < tasteDepth; n += 2)
        {
            if (asBytes[n] == 0)
            {
                count++;
            }
        }

        proportion = (double)count / tasteDepth;
        if (proportion > proportionOfMostSignificantBytesThatMustBeZeroToQualifyAsUtf16)
        {
            // Little endian
            var encoding = Encoding.Unicode;
            var text = encoding.GetString(asBytes);
            return (encoding, text);
        }

        // Finally, a long shot - let's see if we can find "charset=xyz" or "encoding=xyz" to identify the encoding:
        for (var n = 0; n < tasteDepth - 9; n++)
        {
            if (
                ((asBytes[n + 0] == 'c' || asBytes[n + 0] == 'C') && (asBytes[n + 1] == 'h' || asBytes[n + 1] == 'H') && (asBytes[n + 2] == 'a' || asBytes[n + 2] == 'A') && (asBytes[n + 3] == 'r' || asBytes[n + 3] == 'R') && (asBytes[n + 4] == 's' || asBytes[n + 4] == 'S') && (asBytes[n + 5] == 'e' || asBytes[n + 5] == 'E') && (asBytes[n + 6] == 't' || asBytes[n + 6] == 'T') && (asBytes[n + 7] == '='))
                || ((asBytes[n + 0] == 'e' || asBytes[n + 0] == 'E') && (asBytes[n + 1] == 'n' || asBytes[n + 1] == 'N') && (asBytes[n + 2] == 'c' || asBytes[n + 2] == 'C') && (asBytes[n + 3] == 'o' || asBytes[n + 3] == 'O') && (asBytes[n + 4] == 'd' || asBytes[n + 4] == 'D') && (asBytes[n + 5] == 'i' || asBytes[n + 5] == 'I') && (asBytes[n + 6] == 'n' || asBytes[n + 6] == 'N') && (asBytes[n + 7] == 'g' || asBytes[n + 7] == 'G') && (asBytes[n + 8] == '='))
            )
            {
                if (asBytes[n + 0] == 'c' || asBytes[n + 0] == 'C')
                {
                    n += 8;
                }
                else
                {
                    n += 9;
                }

                if (asBytes[n] == '"' || asBytes[n] == '\'')
                {
                    n++;
                }

                var oldn = n;
                while (n < tasteDepth
                       && (asBytes[n] == '_' || asBytes[n] == '-' || (asBytes[n] >= '0' && asBytes[n] <= '9') ||
                           (asBytes[n] >= 'a' && asBytes[n] <= 'z') || (asBytes[n] >= 'A' && asBytes[n] <= 'Z')))
                {
                    n++;
                }

                var nb = new byte[n - oldn];
                Array.Copy(asBytes, oldn, nb, 0, n - oldn);
                try
                {
                    var internalEncString = Encoding.ASCII.GetString(nb);
                    var encoding = Encoding.GetEncoding(internalEncString);
                    var text = encoding.GetString(asBytes);
                    return (encoding, text);
                }
                catch { break; }    // If C# doesn't recognize the name of the encoding, break.
            }
        }

        // If all else fails, the encoding is probably (though not guaranteed to be) the user's local codepage!
        // One might present to the user a list of alternative encodings as shown here:
        // https://stackoverflow.com/questions/8509339/what-is-the-most-common-encoding-of-each-language
        // This option is not available in a cross-platform world.
        throw new ArgumentException("Could not determine the encoding for input string");
    }

    // <summary>
    /// Taken from StackOverflow: https://stackoverflow.com/q/6309379
    ///
    /// Substantially faster (1251 ns) than wrapping with a try-catch and using `Convert` (20,281 ns)
    /// </summary>
    public static bool IsBase64(this string haystack)
        => (haystack.Length % 4 == 0) && _base64Regex.IsMatch(haystack);

    private static readonly Regex _base64Regex = new Regex(@"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.Compiled);

    /// <summary>
    /// Returns true if the entire contents of the string is an ASCII digit (0 1 2 3 4 6 7 8 9)
    /// </summary>
    public static bool IsAsciiDigits(this string haystack)
    {
        if (string.IsNullOrEmpty(haystack))
        {
            return false;
        }

        // Compiled regex is slower, LINQ is slower, direct access by index is slower than the foreach(!).
        foreach (var needle in haystack)
        {
            if (needle < '0' || needle > '9')
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Breaks a string into chunks using the specified separator
    /// </summary>
    /// <param name="toBeChunked"></param>
    /// <param name="separator"></param>
    /// <param name="chunkSize"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string Chunk(this string toBeChunked, string separator, int chunkSize)
    {
        if (chunkSize < 1)
        {
            throw new ArgumentException("Chunk sizes must be greater than zero");
        }

        var numberOfChunks = (toBeChunked.Length / chunkSize) + 1;
        var builderCapacity = toBeChunked.Length + (numberOfChunks * separator.Length) + separator.Length;

        var builder = new StringBuilder(builderCapacity);
        for (var i = 0; i < toBeChunked.Length; i += chunkSize)
        {
            var remainingChars = toBeChunked.Length - i;
            var sliceSize = Math.Min(chunkSize, remainingChars);
            var section = toBeChunked.Substring(i, sliceSize);
            builder.Append(section);

            // This is faster than doing an unconditional truncate outside the loop
            if (remainingChars > sliceSize)
            {
                builder.Append(separator);
            }
        }

        return builder.ToString();
    }
}