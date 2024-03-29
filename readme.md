## Design philosophy
Haystack is meant to be a small library that fills in some of the gaps in the BCL. By definition, it's kind of a dumping ground for little helper methods and wrappers.

## Installation and compatibility
### v2.0
Haystack is available as a nuget package targeting net6.0.

### v1.0
Haystack is available as a nuget package [targeting .NET Standard 1.3](https://docs.microsoft.com/en-us/dotnet/standard/net-standard), which means it's compatible with a fairly wide range of .NET runtimes:

* .NET Core 1.0+
* .NET Framework 4.6+
* Mono 4.6+
* UWP 10+
* Unity 2018.1
* Etc.

## Performance
Generally speaking, if there's more than one obvious way something could be implemented, the best-performing implementation was chosen. There is a performance testing project in the source that indicates where performance testing was done. That said, due to the nature of performance testing, only the best-performing alternative is kept. I.e. there is no performance testing code kept around for the lower-performing alternatives.

Performance results may be found in the `PerformanceResults` directory.

## Versioning
Haystack uses [semantic versioning](http://semver.org/). In a nutshell:

> Given a version number MAJOR.MINOR.PATCH, increment the:
>
> 1. MAJOR version when you make incompatible API changes,
> 2. MINOR version when you add functionality in a backwards-compatible manner, and
> 3. PATCH version when you make backwards-compatible bug fixes.

## Credit
I wrote most of these methods myself. Chances are if you're here, you've written many of them in the past, too, and you're tired of writing them again.

There are some implementations that are cobbled together from other sources. When that's the case, I have left a note in the source code indicating where it was taken from.

## Build
From the top-level directory:

```
dotnet pack -c Release Haystack/Haystack.csproj
```

This should read the nuspec parameters from `Haystack.csproj` and do the right thing.

## Installation and compatibility
### v2.0
Haystack is available as a nuget package targeting net6.0.

### v1.0
Haystack is available as a nuget package [targeting .NET Standard 1.3](https://docs.microsoft.com/en-us/dotnet/standard/net-standard), which means it's compatible with a fairly wide range of .NET runtimes:

* .NET Core 1.0+
* .NET Framework 4.6+
* Mono 4.6+
* UWP 10+
* Unity 2018.1
* Etc.
