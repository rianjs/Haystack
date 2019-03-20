using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess;
using PerformanceTests.Random;
using PerformanceTests.String;

namespace PerformanceTests
{
    class Program
    {
        static void Main(string[] args)
        {
            //BenchmarkRunnerCore.Run(BenchmarkConverter.TypeToBenchmarks(typeof(StringTests)), t => InProcessToolchain.Instance);
            //BenchmarkRunnerCore.Run(BenchmarkConverter.TypeToBenchmarks(typeof(SafeRandomTests)), t => InProcessToolchain.Instance);
            BenchmarkRunnerCore.Run(BenchmarkConverter.TypeToBenchmarks(typeof(AsyncSafeRandomTests)), t => InProcessToolchain.Instance);
        }
    }
}