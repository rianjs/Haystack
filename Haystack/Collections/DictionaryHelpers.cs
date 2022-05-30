using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Haystack.Collections;

public static class DictionaryHelpers
{
    /// <summary>
    /// Compares two dictionaries in an order-insensitive way, and returns true if they have all the same key-value pairs.
    /// </summary>
    /// <remarks>If the left dictionary has a more strict comparer (case-sensitive, for example) than the right dictionary (case-insensitive, for example), then
    /// you may find that collections are considered equal when you don't expect them to be. In this case, make sure the dictionary with the stricter equality
    /// semantics is assigned to `right`.</remarks>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TVal"></typeparam>
    /// <returns></returns>
    public static bool DictionaryEquals<TKey, TVal>(IReadOnlyDictionary<TKey, TVal> left, IReadOnlyDictionary<TKey, TVal> right)
        where TVal : IEquatable<TVal>
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

        if (left.Count != right.Count)
        {
            return false;
        }

        foreach (var (key, val) in left)
        {
            if (!right.TryGetValue(key, out var v))
            {
                return false;
            }

            if (!val.Equals(v))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Keys and values are combined into a single hashcode, and the collection of hashcodes representing each key-value pair is summed and returned
    /// </summary>
    /// <param name="dict"></param>
    /// <param name="keyEqualityOverride">Otherwise if the dictionary is a Dictionary, its Comparer property is used. If it's not a Dictionary,
    /// EqualityComparer.Default is used.</param>
    /// <param name="valueEqualityOverride">Otherwise EqualityComparer.Default is used</param>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TVal"></typeparam>
    /// <returns></returns>
    /// <exception cref="NotSupportedException">ImmutableDictionary is not supported.</exception>
    public static int GetHashCode<TKey, TVal>(
        IReadOnlyDictionary<TKey, TVal> dict,
        IEqualityComparer<TKey> keyEqualityOverride = null,
        IEqualityComparer<TVal> valueEqualityOverride = null)
    {
        if (dict is ImmutableDictionary<TKey, TVal>)
        {
            throw new NotSupportedException("Immutable dictionaries are not supported at this time");
        }

        if (dict is null || dict.Count == 0)
        {
            return 0;
        }

        var keyEquality = keyEqualityOverride ?? (dict is Dictionary<TKey, TVal> typed
            ? typed.Comparer
            : EqualityComparer<TKey>.Default);
        var valEquality = valueEqualityOverride ?? EqualityComparer<TVal>.Default;

        unchecked
        {
            var total = 0;
            foreach (var (key, val) in dict)
            {
                var hashCode = new HashCode();
                hashCode.Add(key, keyEquality);
                hashCode.Add(val, valEquality);
                total += hashCode.ToHashCode();
            }
            return total;
        }
    }
}