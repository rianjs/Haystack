## Release notes

A listing of what each [Nuget package](https://www.nuget.org/packages/Haystack) version represents.

### v1
* 1.0.3 - (2019-04-03) Add `ToSecureString()` extension method on `System.String`.
* 1.0.2 - (2019-03-20)
  * Move existing types to namespaces that more closely mirror the BCL
  * Add a few extensions to `Random.NextDouble()` to allow for floor and ceiling values
  * Add thread-safe wrappers for `Random` behind mockable interfaces for greater safety and testability:
    * `AsyncSafeRandom` / `IAsyncSafeRandom` for use in most places. `Async`-suffixed methods do non-blocking waits, the other methods block threads. You can use the blocking versions -- they cut down about 40 nanonseconds of overhead -- but the moment you add an `async` code path, you should convert all consumers of the active instance to the `async` counterparts. (Maybe this is a footgun for inexperienced developers, and I should get rid of the blocking variants, but at least it's an easy path forward within an application.).
    * `SafeRandom` / `ISafeRandom` for use in NON-`async` code paths, using `lock` under the hood

### v0
* 0.0.3 - (2018-12-23) - Add `string.IsBase64()` extension method + perf tests
* 0.0.2 - (2018-11-25) - Corrected license URL in nuspec file; no functional changes
* 0.0.1 - (2018-11-25) - Initial release
