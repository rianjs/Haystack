using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using Haystack.String;

namespace PerformanceTests.String
{
    public class StringTests
    {
        private const string _someString = "Hello worldHello worldHello worldHello worldHello worldHello worldHello worldHello worldHello world";
        private static readonly string _lower = _someString.ToLowerInvariant();
        private static readonly string _upper = _someString.ToUpperInvariant();
        private static readonly string _copy = new string(_someString);
        private static readonly byte[] _lcBytes = Encoding.UTF8.GetBytes(_lower);
        private static readonly byte[] _ssBytes = Encoding.UTF8.GetBytes(_someString);

        [Benchmark]
        public bool ConstantTimeComparisonEqual()
            => StringExtensions.ConstantTimeEquals(_someString, _copy);

        [Benchmark]
        public bool ConstantTimeComparisonNotEqual()
            => StringExtensions.ConstantTimeEquals(_lower, _upper);

        [Benchmark]
        public bool StringEqualsControl()
            => string.Equals(_someString, _copy, StringComparison.Ordinal);

        [Benchmark]
        public bool StringNotEqualsControl()
            => string.Equals(_lower, _upper, StringComparison.Ordinal);

        private static string _base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(_someString));

        [Benchmark]
        public bool StringIsBase64WithConvert()
        {
            try
            {
                var _ = Convert.FromBase64String(_someString);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        [Benchmark]
        public bool StringIsBase64Regex()
            => _base64.IsBase64();

        private const string _longNumberString = "123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
        private const string _shortNumberString = "1234567890123456789012345678901234567890";
        private static readonly Regex _asciiDigitRegex = new Regex("(^[0-9]+$)", RegexOptions.Compiled);

        [Benchmark]
        public bool IsAsciiDigitsOnlyRegex()
        {
            // Mean: 6,211.1 ns
            return _asciiDigitRegex.IsMatch(_shortNumberString) && _asciiDigitRegex.IsMatch(_longNumberString);
        }

        [Benchmark]
        public bool IsAsciiDigitsOnlyForeachIteration()
        {
            // Winner: Mean 338.7 ns
            foreach (var c in _shortNumberString)
            {
                if (c < '0' || c > '9')
                {
                    return false;
                }
            }

            foreach (var c in _longNumberString)
            {
                if (c < '0' || c > '9')
                {
                    return false;
                }
            }

            return true;
        }

        [Benchmark]
        public bool IsAsciiDigitsOnlyForIteration()
        {
            // Mean: 457.5 ns
            for (var i = 0; i < _shortNumberString.Length; i++)
            {
                var c = _shortNumberString[i];

                if (c < '0' || c > '9')
                {
                    return false;
                }
            }

            for (var i = 0; i < _longNumberString.Length; i++)
            {
                var c = _longNumberString[i];

                if (c < '0' || c > '9')
                {
                    return false;
                }
            }

            return true;
        }

        [Benchmark]
        public bool IsAsciiDigitsOnlyLinqIteration()
        {
            // Mean: 5,152.5 ns
            return _shortNumberString.All(c => c >= '0' && c <= '9')
                && _longNumberString.All(c => c >= '0' && c <= '9');
        }
    }
}