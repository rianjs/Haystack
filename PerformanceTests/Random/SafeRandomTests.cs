using BenchmarkDotNet.Attributes;
using Haystack.Random;

namespace PerformanceTests.Random
{
    public class SafeRandomTests
    {
        private static readonly SafeRandom _random = new SafeRandom();

        [Benchmark]
        public int Next() => _random.Next();

        [Benchmark]
        public int NextMax() => _random.Next(100);

        [Benchmark]
        public int NextMinMax() => _random.Next(-100, 100);

        [Benchmark]
        public double NextDouble() => _random.NextDouble();

        [Benchmark]
        public double NextDoubleMinMax() => _random.NextDouble(long.MinValue, long.MaxValue);

        [Benchmark]
        public double NextDoubleMax() => _random.NextDouble(long.MinValue);

        [Benchmark]
        public void NextBytes()
        {
            var b = new byte[256];
            _random.NextBytes(b);
        }
    }
}