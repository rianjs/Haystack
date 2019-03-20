using System.Collections.Generic;
using Haystack.String;
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

        [Test, TestCaseSource(nameof(TrimStartTestCases))]
        public string TrimStartTests(string trim)
            => _someString.TrimStart(trim);

        public static IEnumerable<ITestCaseData> TrimStartTestCases()
        {
            yield return new TestCaseData("Hello world")
                .SetName("Hello world trimmed off the front")
                .Returns("Hello worldHello worldHello worldHello worldHello worldHello worldHello worldHello world");
            
            yield return new TestCaseData("Foo")
                .SetName("Foo trimmed from the front does nothing")
                .Returns(_someString);
            
            yield return new TestCaseData(null)
                .SetName("Null trimmed from the front does nothing")
                .Returns(_someString);
            
            yield return new TestCaseData(string.Empty)
                .SetName("Empty string trimmed from the front does nothing")
                .Returns(_someString);
        }

        [Test, TestCaseSource(nameof(TrimEndTestCases))]
        public string TrimEndTests(string trim)
            => _someString.TrimEnd(trim);

        public static IEnumerable<ITestCaseData> TrimEndTestCases()
        {
            yield return new TestCaseData("Hello world")
                .SetName("Hello world trimmed off the end")
                .Returns("Hello worldHello worldHello worldHello worldHello worldHello worldHello worldHello world");
            
            yield return new TestCaseData("Foo")
                .SetName("Foo trimmed from the end does nothing")
                .Returns(_someString);
            
            yield return new TestCaseData(null)
                .SetName("Null trimmed from the end does nothing")
                .Returns(_someString);
            
            yield return new TestCaseData(string.Empty)
                .SetName("Empty string trimmed from the end does nothing")
                .Returns(_someString);
        }

        [Test, TestCaseSource(nameof(TrimTestCases))]
        public string TrimTests(string trim)
            => _someString.Trim(trim);
        
        public static IEnumerable<ITestCaseData> TrimTestCases()
        {
            yield return new TestCaseData("Hello world")
                .SetName("Hello world trimmed off both ends")
                .Returns("Hello worldHello worldHello worldHello worldHello worldHello worldHello world");
            
            yield return new TestCaseData("Foo")
                .SetName("Foo trimmed from both ends does nothing")
                .Returns(_someString);
            
            yield return new TestCaseData(null)
                .SetName("Null trimmed from both ends does nothing")
                .Returns(_someString);
            
            yield return new TestCaseData(string.Empty)
                .SetName("Empty string trimmed from both ends does nothing")
                .Returns(_someString);
        }

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
