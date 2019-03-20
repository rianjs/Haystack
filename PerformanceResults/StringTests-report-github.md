``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17763
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=2.2.100
  [Host] : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT


```
|                         Method |         Mean |       Error |      StdDev |
|------------------------------- |-------------:|------------:|------------:|
|    ConstantTimeComparisonEqual |  1,464.14 ns |   8.7814 ns |   7.7845 ns |
| ConstantTimeComparisonNotEqual |  1,461.82 ns |   7.8400 ns |   6.9500 ns |
|            StringEqualsControl |     15.35 ns |   0.0638 ns |   0.0597 ns |
|         StringNotEqualsControl |     11.19 ns |   0.0326 ns |   0.0272 ns |
|      StringIsBase64WithConvert | 19,969.03 ns | 116.9532 ns | 109.3981 ns |
|            StringIsBase64Regex |  1,210.53 ns |  24.6011 ns |  31.9883 ns |
