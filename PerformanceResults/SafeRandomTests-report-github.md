``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17763
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=2.2.100
  [Host] : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT


```
|           Method |        Mean |     Error |    StdDev |      Median |
|----------------- |------------:|----------:|----------:|------------:|
|             Next |    18.98 ns | 0.2507 ns | 0.2222 ns |    18.97 ns |
|          NextMax |    19.19 ns | 0.1767 ns | 0.1653 ns |    19.15 ns |
|       NextMinMax |    20.26 ns | 0.4736 ns | 0.8169 ns |    19.86 ns |
|       NextDouble |    19.27 ns | 0.4098 ns | 0.4208 ns |    19.13 ns |
| NextDoubleMinMax |    19.21 ns | 0.3943 ns | 0.5127 ns |    19.09 ns |
|    NextDoubleMax |    19.79 ns | 0.1733 ns | 0.1536 ns |    19.82 ns |
|        NextBytes | 1,791.20 ns | 7.2225 ns | 6.7559 ns | 1,792.64 ns |
