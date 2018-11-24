using System.Collections.Generic;
using Haystack;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace UnitTests
{
    public class StringExtensionTests
    {
        private const string _someString = "Hello worldHello worldHello worldHello worldHello worldHello worldHello worldHello worldHello world";
        private static readonly string _lower = _someString.ToLowerInvariant();
        private static readonly string _upper = _someString.ToUpperInvariant();
        private static readonly string _copy = string.Copy(_someString);
        
        [Test, TestCaseSource(nameof(ConstantTimeEqualsTestCases))]
        public bool ConstantTimeEqualsTests(string lhs, string rhs)
            => StringExtensions.ConstantTimeEquals(lhs, rhs);

        public static IEnumerable<ITestCaseData> ConstantTimeEqualsTestCases()
        {
            yield return new TestCaseData(_someString, _copy)
                .Returns(true)
                .SetName("Two equal strings returns true");

            yield return new TestCaseData(_lower, _upper)
                .Returns(false)
                .SetName("Two strings that would pass a case-insensitive comparison return false");

            yield return new TestCaseData(_someString, "different string")
                .Returns(false)
                .SetName("Two strings of different length return false");

            yield return new TestCaseData(null, null)
                .Returns(true)
                .SetName("nulls return true");

            yield return new TestCaseData("      ", null)
                .Returns(false)
                .SetName("Whitespace and null are different");
        }
    }
}
