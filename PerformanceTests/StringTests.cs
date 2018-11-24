using System;
using BenchmarkDotNet.Attributes;
using Haystack;

namespace PerformanceTests
{
    public class StringTests
    {
        private const string _someString = "Hello worldHello worldHello worldHello worldHello worldHello worldHello worldHello worldHello world";
        private static readonly string _lower = _someString.ToLowerInvariant();
        private static readonly string _upper = _someString.ToUpperInvariant();
        private static readonly string _copy = new string(_someString);
        
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
    }
}