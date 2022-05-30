using System;
using System.Collections.Generic;
using System.Linq;

namespace Haystack.Collections;

public static class CollectionHelpers
{
    /// <summary> Commutative, stable, order-independent hashing for collections of collections. </summary>
    public static int GetCollectionHashCode<T>(this IEnumerable<T> collection)
        where T : IEquatable<T>
    {
        unchecked
        {
            if (collection is null)
            {
                return 0;
            }

            var total = 0;
            foreach (var element in collection)
            {
                total += element?.GetHashCode() ?? 0;
            }
            return total;
        }
    }

    /// <summary>
    /// If order is not significant, duplicates will be ignored, because at the end of the day, we have to treat each collection as a set.
    /// If order IS significant, we just compare each element in the sequence one by one, and return false when there's a mismatch.
    /// </summary>
    /// <remarks>O(1) when collections are not the same size. O(n) when order is significant. O(n) + O(m) when order is not significant.</remarks>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="orderSignificant"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool CollectionEquals<T>(this IReadOnlyCollection<T> left, IReadOnlyCollection<T> right, bool orderSignificant = false)
        where T : IEquatable<T>
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null && right is not null)
        {
            return false;
        }

        if (left is not null && right is null)
        {
            return false;
        }

        if (left!.Count != right!.Count)
        {
            return false;
        }

        if (orderSignificant)
        {
            return left.SequenceEqual(right);
        }

        var leftSet = new HashSet<T>(left);
        var rightSet = new HashSet<T>(right);
        return leftSet.SetEquals(rightSet);
    }
}