using System.Collections;

namespace Haystack.Collections;

public static class EnumerableExtensions
{
    /// <summary>
    /// If the IEnumerable is null or empty, it returns true. This method makes no assertions about the values in the IEnumerable.
    /// - List<T> with full of nulls will return false, but an empty List with a specified capacity will return true
    /// - Arrays with non-zero size will return false, even if there's nothing in the allocated memory locations
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this IEnumerable e)
    {
        if (e is null)
        {
            return true;
        }

        foreach (var _ in e)
        {
            return false;
        }

        return true;
    }
}