using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Haystack
{
    public static class StringExtensions
    {
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

        public static string Trim(this string input, string prefixAndSuffixPrefixToRemove, StringComparison comparisonType) =>
            input.TrimStart(prefixAndSuffixPrefixToRemove, comparisonType).TrimEnd(prefixAndSuffixPrefixToRemove, comparisonType);

        public static bool Contains(this string haystack, string needle, StringComparison comp)
            => haystack.IndexOf(needle, comp) >= 0;

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
            var hashedBytes = SHA1.Create().ComputeHash(asBytes).Take(16).ToArray();
            return new Guid(hashedBytes);
        }

        /// <summary>
        /// A string comparison method safe from timing attacks
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static bool ConstantTimeEquals(this string control, string comparison, Encoding encoding)
        {
            if (control == null && comparison != null || control != null && comparison == null)
            {
                return false;
            }

            if (ReferenceEquals(control, comparison))
            {
                return true;
            }

            if (control.Length != comparison.Length)
            {
                return false;
            }

            using (var hasher = MD5.Create())
            {
                var leftBytes = hasher.ComputeHash(encoding.GetBytes(control));
                var rightBytes = hasher.ComputeHash(encoding.GetBytes(comparison));

                // This is the traditional bitwise XOR implementation:
                // var result = 0;
                // for (var i = 0; i < lhs.Length; i++)
                // {
                //     result |= lhs[i] ^ rhs[i];
                // }
                //
                // return result == 0;
            
                // This more readable implementation has the same performance as the traditional implementation, even with optimizations and inlining
                // turned off.
                var same = true;
                for (var i = 0; i < control.Length; i++)
                {
                    if (control[i] != comparison[i])
                    {
                        same = false;
                    }
                }
            
                return same;
            }
        }

        /// <summary>
        /// <inheritdoc cref="ConstantTimeEquals(string,string,System.Text.Encoding)"/> using UTF-8 encoding
        /// </summary>
        public static bool ConstantTimeEquals(this string control, string comparison)
            => ConstantTimeEquals(control, comparison, Encoding.UTF8);
        
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
    }
}