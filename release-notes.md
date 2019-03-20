## Release notes

A listing of what each [Nuget package](https://www.nuget.org/packages/Haystack) version represents.

### v1
* 1.0.1 - (2019-03-20)
  * Move existing types to namespaces that more closely mirror the BCL
  * Add a few extensions to `Random.NextDouble()` to allow for floor and ceiling values
  * Add thread-safe wrappers for `Random` behind mockable interfaces for greater safety and testability:
    * `SafeRandom`/`ISafeRandom` for use in _non-`async`_ code paths.
    * `AsyncSafeRandom`/`IAsyncSafeRandom for use everywhere else

### v0
* 0.0.3 - (2018-12-23) - Add `string.IsBase64()` extension method + perf tests
* 0.0.2 - (2018-11-25) - Corrected license URL in nuspec file; no functional changes
* 0.0.1 - (2018-11-25) - Initial release
