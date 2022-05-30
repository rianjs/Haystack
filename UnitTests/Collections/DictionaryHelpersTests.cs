using System;
using System.Collections.Generic;
using System.Linq;
using Haystack.Collections;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace UnitTests.Collections;

public class DictionaryHelpersTests
{
    [Test, TestCaseSource(nameof(EqualsHappyPathTestCases))]
    public bool EqualsHappyPathTest<TKey, TVal>(IReadOnlyDictionary<TKey, TVal> left, IReadOnlyDictionary<TKey, TVal> right)
        where TVal : IEquatable<TVal>
        => DictionaryHelpers.DictionaryEquals(left, right);

    public static IEnumerable<ITestCaseData> EqualsHappyPathTestCases()
    {
        yield return new TestCaseData(GetDictionary(10), ReverseDict(GetDictionary(10)))
            .Returns(true)
            .SetName("The same dictionary twice are equal to each other");

        yield return new TestCaseData(GetDictionary(10), ReverseDict(GetDictionary(10)))
            .Returns(true)
            .SetName("Dictionary is equal to its reversed equivalent");

        yield return new TestCaseData(GetDictionary(100), ReverseDict(GetDictionary(100)))
            .Returns(true)
            .SetName("Big dictionary is equal to its not-exactly-reversed equivalent");

        var ordinal = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            { "hello", "heLLo" },
        };
        var ordinalIgnoreCase = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "HELLO", "heLLo" },
        };

        yield return new TestCaseData(ordinal, ordinalIgnoreCase)
            .Returns(true)
            .SetName("Dictionaries with the same values but different IEqualityComparers return true when the more permissive dictionary is assigned to right");
    }

    [Test, TestCaseSource(nameof(GetHashCodeHappyPathTestCases))]
    public void GetHashCodeHappyPathTest<TKey, TVal>(IReadOnlyDictionary<TKey, TVal> left, IReadOnlyDictionary<TKey, TVal> right)
    {
        var leftHash = DictionaryHelpers.GetHashCode(left);
        var rightHash = DictionaryHelpers.GetHashCode(right);
        Assert.AreEqual(leftHash, rightHash);
    }

    public static IEnumerable<ITestCaseData> GetHashCodeHappyPathTestCases()
    {
        yield return new TestCaseData(null, new Dictionary<int, string>())
            .SetName("Null collection compared with empty collection has the same hash code");

        yield return new TestCaseData(GetDictionary(10), ReverseDict(GetDictionary(10)))
            .SetName("The same dictionary twice has the same hash code");

        yield return new TestCaseData(GetDictionary(10), ReverseDict(GetDictionary(10)))
            .SetName("Dictionary's hash code is the same as its reversed equivalent");

        yield return new TestCaseData(GetDictionary(100), ReverseDict(GetDictionary(100)))
            .SetName("Big dictionary's hash code is equal to its not-exactly-reversed equivalent");
    }

    [Test, TestCaseSource(nameof(EqualsUnhappyPathTestCases))]
    public bool EqualsUnhappyPathTest<TKey, TVal>(IReadOnlyDictionary<TKey, TVal> left, IReadOnlyDictionary<TKey, TVal> right)
        where TVal : IEquatable<TVal>
        => DictionaryHelpers.DictionaryEquals(left, right);

    public static IEnumerable<ITestCaseData> EqualsUnhappyPathTestCases()
    {
        yield return new TestCaseData(null, GetDictionary(10))
            .Returns(false)
            .SetName("Null collection compared with empty collection returns false");

        yield return new TestCaseData(new Dictionary<int, string>(), GetDictionary(10))
            .Returns(false)
            .SetName("Empty dictionary compared with non-empty dictionary returns false");

        yield return new TestCaseData(GetDictionary(1), GetDictionary(10))
            .Returns(false)
            .SetName("Dictionaries of different sizes return false");

        var ordinal = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            { "hello", "heLLo" },
        };
        var ordinalIgnoreCase = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "HELLO", "heLLo" },
        };

        yield return new TestCaseData(ordinalIgnoreCase, ordinal)
            .Returns(false)
            .SetName("Dictionaries with the same values but different IEqualityComparers return false when the more permissive dictionary is assigned to left");
    }

    [Test, TestCaseSource(nameof(GetHashCodeUnhappyPathTestCases))]
    public void GetHashCodeUnhappyPathTest<TKey, TVal>(IReadOnlyDictionary<TKey, TVal> left, IReadOnlyDictionary<TKey, TVal> right)
    {
        var leftHash = DictionaryHelpers.GetHashCode(left);
        var rightHash = DictionaryHelpers.GetHashCode(right);
        Assert.AreNotEqual(leftHash, rightHash);
    }

    public static IEnumerable<ITestCaseData> GetHashCodeUnhappyPathTestCases()
    {
        yield return new TestCaseData(new Dictionary<int, string>(), GetDictionary(10))
            .SetName("Empty dictionary has different hash code than non-empty dictionary");

        yield return new TestCaseData(GetDictionary(1), GetDictionary(10))
            .SetName("Dictionaries of different sizes have different hash codes");

        var ordinal = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            { "hEllo", "heLLo" },
        };
        var ordinalIgnoreCase = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "hEllo", "heLLo" },
        };

        yield return new TestCaseData(ordinal, ordinalIgnoreCase)
            .SetName("Dictionaries with the same values but different IEqualityComparers have different hash codes");
    }

    private static IReadOnlyDictionary<int, string> GetDictionary(int size)
        => Enumerable.Range(0, size).ToDictionary(i => i, i => i.ToString());

    private static IReadOnlyDictionary<TKey, TVal> ReverseDict<TKey, TVal>(IReadOnlyDictionary<TKey, TVal> d)
    {
        var l = d.ToList();
        l.Reverse();
        return l.ToDictionary(i => i.Key, i => i.Value);
    }
}