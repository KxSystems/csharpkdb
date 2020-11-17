using System.Threading.Tasks;
using NUnit.Framework;

namespace kx.Test.Types
{
    [TestFixture]
    public class MonthTests
    {
        [Test]
        public void MonthInitialisesWithInt32()
        {
            c.Month month = new c.Month(1);

            Assert.IsNotNull(month);
        }

        [Test]
        public void MonthToStringReturnsExpectedString()
        {
            const string expected = "2003-12";

            c.Month month = new c.Month(47);

            Assert.AreEqual(expected, month.ToString());
        }

        [Test]
        public void MonthToStringReturnsExpectedStringForMinInt32()
        {
            const string expected = "";

            c.Month month = new c.Month(int.MinValue);

            Assert.AreEqual(expected, month.ToString());
        }

        [Test]
        public void MonthGetHashCodeReturnsExpectedHash()
        {
            const int expected = 47;

            c.Month month = new c.Month(expected);

            Assert.AreEqual(expected, month.GetHashCode());
        }

        [Test]
        public void MonthEqualsReturnsFalseIfOtherIsNull()
        {
            c.Month month = new c.Month(47);
            c.Month other = null;

            Assert.IsFalse(month.Equals(other));
        }

        [Test]
        public void MonthEqualsReturnsFalseIfOtherIsNotTypeOfMonth()
        {
            c.Month month = new c.Month(47);
            Task other = new Task(() => { });

            Assert.IsFalse(month.Equals(other));
        }

        [Test]
        public void MonthEqualsReturnsTrueIfOtherIsSameReference()
        {
            c.Month month = new c.Month(47);
            c.Month other = month;

            Assert.IsTrue(month.Equals(other));
        }

        [Test]
        public void MonthEqualsReturnsTrueIfOtherIsSameValue()
        {
            c.Month month = new c.Month(47);
            c.Month other = new c.Month(47);

            Assert.IsTrue(month.Equals(other));
        }

        [Test]
        public void MonthEqualsReturnsFalseIfOtherIsDifferentValue()
        {
            c.Month month = new c.Month(47);
            c.Month other = new c.Month(45);

            Assert.IsFalse(month.Equals(other));
        }

        [Test]
        public void MonthCompareToReturnsOneIfOtherIsNull()
        {
            const int expected = 1;

            c.Month month = new c.Month(47);
            c.Month other = null;

            Assert.AreEqual(expected, month.CompareTo(other));
        }

        [Test]
        public void MonthCompareToReturnsOneIfOtherIsNotTypeOfMonth()
        {
            const int expected = 1;

            c.Month month = new c.Month(47);
            Task other = new Task(() => { });

            Assert.AreEqual(expected, month.CompareTo(other));
        }

        [Test]
        public void MonthCompareToReturnsOneIfOtherIsLessThanValue()
        {
            const int expected = 1;

            c.Month month = new c.Month(47);
            c.Month other = new c.Month(46);

            Assert.AreEqual(expected, month.CompareTo(other));
        }

        [Test]
        public void MonthCompareToReturnsZeroIfOtherIsSameReference()
        {
            const int expected = 0;

            c.Month month = new c.Month(47);
            c.Month other = month;

            Assert.AreEqual(expected, month.CompareTo(other));
        }

        [Test]
        public void MonthCompareToReturnsZeroIfOtherIsSameValue()
        {
            const int expected = 0;

            c.Month month = new c.Month(47);
            c.Month other = new c.Month(47);

            Assert.AreEqual(expected, month.CompareTo(other));
        }

        [Test]
        public void MonthCompareToReturnsMinusOneIfOtherIsMoreThanValue()
        {
            const int expected = -1;

            c.Month month = new c.Month(47);
            c.Month other = new c.Month(48);

            Assert.AreEqual(expected, month.CompareTo(other));
        }
        [Test]
        public void MonthEqualsOperatorReturnsTrueIfLeftAndRightAreSameValue()
        {
            c.Month left = new c.Month(47);
            c.Month right = new c.Month(47);

            Assert.IsTrue(left == right);
        }

        [Test]
        public void MonthEqualsOperatorReturnsTrueIfLeftAndRightAreSameReference()
        {
            c.Month left = new c.Month(47);
            c.Month right = left;

            Assert.IsTrue(left == right);
        }

        [Test]
        public void MonthEqualsOperatorReturnsFalseIfLeftAndRightAreNotSameValue()
        {
            c.Month left = new c.Month(47);
            c.Month right = new c.Month(46);

            Assert.IsFalse(left == right);
        }

        [Test]
        public void MonthEqualsOperatorReturnsTrueIfLeftAndRightAreNull()
        {
            c.Month left = null;
            c.Month right = null;

            Assert.IsTrue(left == right);
        }

        [Test]
        public void MonthEqualsOperatorReturnsFalseIfLeftIsNullAndRightIsNotNull()
        {
            c.Month left = null;
            c.Month right = new c.Month(47);

            Assert.IsFalse(left == right);
        }

        [Test]
        public void MonthEqualsOperatorReturnsFalseIfLeftIsNotNullAndRightIsNull()
        {
            c.Month left = new c.Month(47);
            c.Month right = null;

            Assert.IsFalse(left == right);
        }

        [Test]
        public void MonthNotEqualsOperatorReturnsFalseIfLeftAndRightAreSameValue()
        {
            c.Month left = new c.Month(47);
            c.Month right = new c.Month(47);

            Assert.IsFalse(left != right);
        }

        [Test]
        public void MonthNotEqualsOperatorReturnsFalseIfLeftAndRightAreSameReference()
        {
            c.Month left = new c.Month(47);
            c.Month right = left;

            Assert.IsFalse(left != right);
        }

        [Test]
        public void MonthNotEqualsOperatorReturnsTrueIfLeftAndRightAreNotSameValue()
        {
            c.Month left = new c.Month(47);
            c.Month right = new c.Month(46);

            Assert.IsTrue(left != right);
        }

        [Test]
        public void MonthNotEqualsOperatorReturnsFalseIfLeftAndRightAreNull()
        {
            c.Month left = null;
            c.Month right = null;

            Assert.IsFalse(left != right);
        }

        [Test]
        public void MonthNotEqualsOperatorReturnsTrueIfLeftIsNullAndRightIsNotNull()
        {
            c.Month left = null;
            c.Month right = new c.Month(47);

            Assert.IsTrue(left != right);
        }

        [Test]
        public void MonthNotEqualsOperatorReturnsTrueIfLeftIsNotNullAndRightIsNull()
        {
            c.Month left = new c.Month(47);
            c.Month right = null;

            Assert.IsTrue(left != right);
        }

        [Test]
        public void MonthLessThanOperatorReturnsFalseIfLeftIsGreaterThanRight()
        {
            c.Month left = new c.Month(47);
            c.Month right = new c.Month(46);

            Assert.IsFalse(left < right);
        }

        [Test]
        public void MonthLessThanOperatorReturnsFalseIfLeftIsSameValueAsRight()
        {
            c.Month left = new c.Month(47);
            c.Month right = new c.Month(47);

            Assert.IsFalse(left < right);
        }

        [Test]
        public void MonthLessThanOperatorReturnsFalseIfLeftIsSameReferenceAsRight()
        {
            c.Month left = new c.Month(47);
            c.Month right = left;

            Assert.IsFalse(left < right);
        }

        [Test]
        public void MonthLessThanOperatorReturnsTrueIfLeftIsLessThanRight()
        {
            c.Month left = new c.Month(47);
            c.Month right = new c.Month(48);

            Assert.IsTrue(left < right);
        }

        [Test]
        public void MonthLessThanOperatorReturnsFalseIfLeftAndRightAreNull()
        {
            c.Month left = null;
            c.Month right = null;

            Assert.IsFalse(left < right);
        }

        [Test]
        public void MonthLessThanOperatorReturnsTrueIfLeftIsNullAndRightIsNotNull()
        {
            c.Month left = null;
            c.Month right = new c.Month(46);

            Assert.IsTrue(left < right);
        }

        [Test]
        public void MonthLessThanOperatorReturnsFalseIfLeftIsNotNullAndRightIsNull()
        {
            c.Month left = new c.Month(47);
            c.Month right = null;

            Assert.IsFalse(left < right);
        }

        [Test]
        public void MonthGreaterThanOperatorReturnsTrueIfLeftIsGreaterThanRight()
        {
            c.Month left = new c.Month(47);
            c.Month right = new c.Month(46);

            Assert.IsTrue(left > right);
        }

        [Test]
        public void MonthGreaterThanOperatorReturnsFalseIfLeftIsSameValueAsRight()
        {
            c.Month left = new c.Month(47);
            c.Month right = new c.Month(47);

            Assert.IsFalse(left > right);
        }

        [Test]
        public void MonthGreaterThanOperatorReturnsFalseIfLeftIsSameReferenceAsRight()
        {
            c.Month left = new c.Month(47);
            c.Month right = left;

            Assert.IsFalse(left > right);
        }

        [Test]
        public void MonthGreaterThanOperatorReturnsFalseIfLeftIsLessThanRight()
        {
            c.Month left = new c.Month(47);
            c.Month right = new c.Month(48);

            Assert.IsFalse(left > right);
        }

        [Test]
        public void MonthGreaterThanOperatorReturnsFalseIfLeftAndRightAreNull()
        {
            c.Month left = null;
            c.Month right = null;

            Assert.IsFalse(left > right);
        }

        [Test]
        public void MonthGreaterThanOperatorReturnsFalseIfLeftIsNullAndRightIsNotNull()
        {
            c.Month left = null;
            c.Month right = new c.Month(46);

            Assert.IsFalse(left > right);
        }

        [Test]
        public void MonthGreaterThanOperatorReturnsTrueIfLeftIsNotNullAndRightIsNull()
        {
            c.Month left = new c.Month(47);
            c.Month right = null;

            Assert.IsTrue(left > right);
        }

        [Test]
        public void MonthLessThanOrEqualOperatorReturnsFalseIfLeftIsGreaterThanRight()
        {
            c.Month left = new c.Month(47);
            c.Month right = new c.Month(46);

            Assert.IsFalse(left <= right);
        }

        [Test]
        public void MonthLessThanOrEqualOperatorReturnsTrueIfLeftIsSameValueAsRight()
        {
            c.Month left = new c.Month(47);
            c.Month right = new c.Month(47);

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void MonthLessThanOrEqualOperatorReturnsTrueIfLeftIsSameReferenceAsRight()
        {
            c.Month left = new c.Month(47);
            c.Month right = left;

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void MonthLessThanOrEqualOperatorReturnsTrueIfLeftIsLessThanRight()
        {
            c.Month left = new c.Month(47);
            c.Month right = new c.Month(48);

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void MonthLessThanOrEqualOperatorReturnsTrueIfLeftAndRightAreNull()
        {
            c.Month left = null;
            c.Month right = null;

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void MonthLessThanOrEqualOperatorReturnsTrueIfLeftIsNullAndRightIsNotNull()
        {
            c.Month left = null;
            c.Month right = new c.Month(46);

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void MonthLessThanOrEqualOperatorReturnsFalseIfLeftIsNotNullAndRightIsNull()
        {
            c.Month left = new c.Month(47);
            c.Month right = null;

            Assert.IsFalse(left <= right);
        }

        [Test]
        public void MonthGreaterThanOrEqualOperatorReturnsTrueIfLeftIsGreaterThanRight()
        {
            c.Month left = new c.Month(47);
            c.Month right = new c.Month(46);

            Assert.IsTrue(left >= right);
        }

        [Test]
        public void MonthGreaterThanOrEqualOperatorReturnsTrueIfLeftIsSameValueAsRight()
        {
            c.Month left = new c.Month(47);
            c.Month right = new c.Month(47);

            Assert.IsTrue(left >= right);
        }

        [Test]
        public void MonthGreaterThanOrEqualOperatorReturnsTrueIfLeftIsSameReferenceAsRight()
        {
            c.Month left = new c.Month(47);
            c.Month right = left;

            Assert.IsTrue(left >= right);
        }

        [Test]
        public void MonthGreaterThanOrEqualOperatorReturnsFalseIfLeftIsLessThanRight()
        {
            c.Month left = new c.Month(47);
            c.Month right = new c.Month(48);

            Assert.IsFalse(left >= right);
        }

        [Test]
        public void MonthGreaterThanOrEqualOperatorReturnsTrueIfLeftAndRightAreNull()
        {
            c.Month left = null;
            c.Month right = null;

            Assert.IsTrue(left >= right);
        }

        [Test]
        public void MonthGreaterThanOrEqualOperatorReturnsFalseIfLeftIsNullAndRightIsNotNull()
        {
            c.Month left = null;
            c.Month right = new c.Month(46);

            Assert.IsFalse(left >= right);
        }

        [Test]
        public void MonthGreaterThanOrEqualOperatorReturnsTrueIfLeftIsNotNullAndRightIsNull()
        {
            c.Month left = new c.Month(47);
            c.Month right = null;

            Assert.IsTrue(left >= right);
        }
    }
}
