using System.Collections;
using System.Collections.Generic;
using Haystack.Collections;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace UnitTests.Collections
{
    public class EnumerableExtensionsTests
    {
        [Test, TestCaseSource(nameof(IsNullOrEmptyTestCases))]
        public bool IsNullOrEmptyTests(IEnumerable a)
        {
            return a.IsNullOrEmpty();
        }

        public static IEnumerable<ITestCaseData> IsNullOrEmptyTestCases()
        {
            var four = new List<string>(4);
            yield return new TestCaseData(four)
                .SetName("Empty list with specified capacity returns true")
                .Returns(true);
            
            var one = new List<string>
            {
                "Foo"
            };
            yield return new TestCaseData(one)
                .SetName("List with one element returns false")
                .Returns(false);
            
            List<int> nullList = null;
            yield return new TestCaseData(nullList)
                .SetName("list set to null returns true")
                .Returns(true);
        }

        [Test]
        public void ArrayIsNullOrEmptyTests()
        {
            // Arrays are a bit different than Lists in this regard.
            var emptyStringArray = new string[2];
            Assert.IsFalse(emptyStringArray.IsNullOrEmpty());

            string[] nullStringArray = null;
            Assert.IsTrue(nullStringArray.IsNullOrEmpty());

            var stringArrayWithContents = new[]
            {
                "Hello",
                "World",
            };
            Assert.IsFalse(stringArrayWithContents.IsNullOrEmpty());
        }
    }
}