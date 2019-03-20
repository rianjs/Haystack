using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Haystack.Random;

namespace PerformanceTests.Random
{
    public class AsyncSafeRandomTests
    {
        private static readonly AsyncSafeRandom _random = new AsyncSafeRandom();

        [Benchmark]
        public async Task<int> NextAsync() => await _random.NextAsync();

        [Benchmark]
        public int Next() => _random.Next();

        [Benchmark]
        public async Task<int> NextMaxAsync() => await _random.NextAsync(100);

        [Benchmark]
        public int NextMax() => _random.Next(100);

        [Benchmark]
        public async Task<int> NextMinMaxAsync() => await _random.NextAsync(-100, 100);

        [Benchmark]
        public int NextMinMax() => _random.Next(-100, 100);

        [Benchmark]
        public async Task<double> NextDoubleAsync() => await _random.NextDoubleAsync();

        [Benchmark]
        public double NextDouble() => _random.NextDouble();

        [Benchmark]
        public async Task<double> NextDoubleMinMaxAsync() => await _random.NextDoubleAsync(long.MinValue, long.MaxValue);

        [Benchmark]
        public double NextDoubleMinMax() => _random.NextDouble(long.MinValue, long.MaxValue);

        [Benchmark]
        public async Task<double> NextDoubleMaxAsync() => await _random.NextDoubleAsync(long.MinValue);

        [Benchmark]
        public double NextDoubleMax() => _random.NextDouble(long.MinValue);

        [Benchmark]
        public async Task NextBytesAsync()
        {
            var b = new byte[16];
            await _random.NextBytesAsync(b);
        }

        [Benchmark]
        public void NextBytes()
        {
            var b = new byte[16];
            _random.NextBytes(b);
        }
    }
}