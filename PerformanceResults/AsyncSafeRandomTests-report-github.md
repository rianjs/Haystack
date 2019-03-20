``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17763
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=2.2.100
  [Host] : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT


```
|                Method |        Mean |      Error |     StdDev |
|---------------------- |------------:|-----------:|-----------:|
|             NextAsync |    88.77 ns |  1.0290 ns |  0.9625 ns |
|          NextMaxAsync |    86.88 ns |  1.4005 ns |  1.2415 ns |
|       NextMinMaxAsync |    89.94 ns |  1.6393 ns |  1.5334 ns |
|       NextDoubleAsync |    82.01 ns |  0.4481 ns |  0.3499 ns |
| NextDoubleMinMaxAsync |    85.16 ns |  1.3849 ns |  1.2277 ns |
|    NextDoubleMaxAsync |    88.40 ns |  1.8503 ns |  2.5939 ns |
|             NextBytes | 1,951.49 ns | 39.8898 ns | 47.4860 ns |
