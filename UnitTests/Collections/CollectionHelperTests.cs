using System;
using System.Collections.Generic;
using System.Linq;
using Haystack.Collections;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace UnitTests.Collections;

public class CollectionHelpersTests
{
    private static List<int> GetVals(int limit)
        => Enumerable.Range(0, limit).ToList();

    [Test, TestCaseSource(nameof(EqualsTestCases))]
    public bool EqualsTest<T>(IReadOnlyCollection<T> left, IReadOnlyCollection<T> right)
        where T : IEquatable<T>
        => left.CollectionEquals(right);

    public static IEnumerable<ITestCaseData> EqualsTestCases()
    {
        var vals = GetVals(10);
        var reversed = GetVals(10);
        reversed.Reverse();

        yield return new TestCaseData(vals, vals)
            .Returns(true)
            .SetName("The same collection twice are equal to each other");

        yield return new TestCaseData(vals, reversed)
            .Returns(true)
            .SetName("Collection is equal to its reversed equivalent");

        var bigVals = GetVals(100);
        var bigReversed = GetVals(100);
        bigReversed.Reverse();

        yield return new TestCaseData(bigVals, bigReversed)
            .Returns(true)
            .SetName("Big collection is equal to its not-exactly-reversed equivalent");

        var strings = new List<string>
        {
            "Hello",
            "hello",
        };
        var differentCasing = new List<string>
        {
            "heLLo",
            "HELLO",
        };

        yield return new TestCaseData(strings, differentCasing)
            .Returns(false)
            .SetName("Collection with the same values but different casing values return true when the more permissive dictionary is assigned to right");
    }

    [Test, TestCaseSource(nameof(OrderSignificantTestCases))]
    public bool OrderSignificantTests<T>(IReadOnlyCollection<T> left, IReadOnlyCollection<T> right)
        where T : IEquatable<T>
        => left.CollectionEquals(right, orderSignificant: true);

    public static IEnumerable<ITestCaseData> OrderSignificantTestCases()
    {
        var vals = GetVals(10);
        var reversed = GetVals(10);
        reversed.Reverse();

        yield return new TestCaseData(vals, vals)
            .Returns(true)
            .SetName("The same collection twice are equal to each other");

        yield return new TestCaseData(vals, reversed)
            .Returns(false)
            .SetName("Collection is equal to its reversed equivalent");

        yield return new TestCaseData(GetVals(100), GetVals(100))
            .Returns(true)
            .SetName("Big collection twice with different reference returns true");

        var bigVals = GetVals(100);
        var bigReversed = GetVals(100);
        bigReversed.Reverse();
        yield return new TestCaseData(bigVals, bigReversed)
            .Returns(false)
            .SetName("Big collection is equal to its not-exactly-reversed equivalent");
    }

    [Test, TestCaseSource(nameof(GetHashCodeHappyPathTestCases))]
    public void GetHashCodeHappyPathTests<T>(List<T> left, List<T> right)
        where T : IEquatable<T>
    {
        var leftHash = left.GetCollectionHashCode();
        var rightHash = right.GetCollectionHashCode();
        Assert.AreEqual(leftHash, rightHash);
    }

    public static IEnumerable<ITestCaseData> GetHashCodeHappyPathTestCases()
    {
        var vals = GetVals(10);
        var reversed = GetVals(10);
        reversed.Reverse();

        yield return new TestCaseData(vals, vals)
            .SetName("The same collection twice are equal to each other");

        yield return new TestCaseData(vals, reversed)
            .SetName("Collection is equal to its reversed equivalent");

        var bigVals = GetVals(100);
        var bigReversed = GetVals(100);
        bigReversed.Reverse();

        yield return new TestCaseData(bigVals, bigReversed)
            .SetName("Big collection is equal to its not-exactly-reversed equivalent");
    }

    [Test, TestCaseSource(nameof(GetHashCodeUnhappyPathTestCases))]
    public void GetHashCodeUnhappyPathTests<T>(List<T> left, List<T> right)
        where T : IEquatable<T>
    {
        var leftHash = left.GetCollectionHashCode();
        var rightHash = right.GetCollectionHashCode();
        Assert.AreNotEqual(leftHash, rightHash);
    }

    public static IEnumerable<ITestCaseData> GetHashCodeUnhappyPathTestCases()
    {
        var strings = new List<string>
        {
            "Hello",
            "hello",
        };
        var differentCasing = new List<string>
        {
            "heLLo",
            "HELLO",
        };

        yield return new TestCaseData(strings, differentCasing)
            .SetName("Collection with the same values but different casing values return true when the more permissive dictionary is assigned to right");
    }
}