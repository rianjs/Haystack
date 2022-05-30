using BenchmarkDotNet.Running;
using PerformanceTests.Random;
using PerformanceTests.String;

namespace PerformanceTests;

class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run(typeof(StringTests).Assembly);
        BenchmarkRunner.Run(typeof(SafeRandomTests).Assembly);
        BenchmarkRunner.Run(typeof(AsyncSafeRandomTests).Assembly);
    }
}