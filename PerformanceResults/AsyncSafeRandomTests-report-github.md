``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17763
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=2.2.100
  [Host] : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT


```
|                Method |      Mean |     Error |    StdDev |
|---------------------- |----------:|----------:|----------:|
|             NextAsync |  86.54 ns | 1.3553 ns | 1.2678 ns |
|                  Next |  43.13 ns | 0.0887 ns | 0.0830 ns |
|          NextMaxAsync |  85.98 ns | 0.3077 ns | 0.2878 ns |
|               NextMax |  45.18 ns | 0.1513 ns | 0.1415 ns |
|       NextMinMaxAsync |  89.85 ns | 0.3707 ns | 0.3286 ns |
|            NextMinMax |  45.74 ns | 0.0922 ns | 0.0770 ns |
|       NextDoubleAsync |  83.81 ns | 0.4385 ns | 0.4102 ns |
|            NextDouble |  45.31 ns | 0.0878 ns | 0.0779 ns |
| NextDoubleMinMaxAsync |  85.09 ns | 0.7812 ns | 0.7307 ns |
|      NextDoubleMinMax |  44.57 ns | 0.0949 ns | 0.0888 ns |
|    NextDoubleMaxAsync |  85.12 ns | 0.4435 ns | 0.3932 ns |
|         NextDoubleMax |  44.35 ns | 0.0761 ns | 0.0712 ns |
|        NextBytesAsync | 185.04 ns | 1.0035 ns | 0.9387 ns |
|             NextBytes | 152.87 ns | 1.8456 ns | 1.6361 ns |
