using System.Threading.Tasks;
using NUnit.Framework;

namespace kx.Test.Types
{
    [TestFixture]
    public class MinuteTests
    {
        [Test]
        public void MinuteInitialisesWithInt32()
        {
            c.Minute minute = new c.Minute(1);

            Assert.IsNotNull(minute);
        }

        [Test]
        public void MinuteToStringReturnsExpectedString()
        {
            const string expected = "00:47";

            c.Minute minute = new c.Minute(47);

            Assert.AreEqual(expected, minute.ToString());
        }

        [Test]
        public void MinuteToStringReturnsExpectedStringForMinInt32()
        {
            const string expected = "";

            c.Minute minute = new c.Minute(int.MinValue);

            Assert.AreEqual(expected, minute.ToString());
        }

        [Test]
        public void MinuteGetHashCodeReturnsExpectedHash()
        {
            const int expected = 47;

            c.Minute minute = new c.Minute(expected);

            Assert.AreEqual(expected, minute.GetHashCode());
        }

        [Test]
        public void MinuteEqualsReturnsFalseIfOtherIsNull()
        {
            c.Minute minute = new c.Minute(47);
            c.Minute other = null;

            Assert.IsFalse(minute.Equals(other));
        }

        [Test]
        public void MinuteEqualsReturnsFalseIfOtherIsNotTypeOfMinute()
        {
            c.Minute minute = new c.Minute(47);
            Task other = new Task(() => { });

            Assert.IsFalse(minute.Equals(other));
        }

        [Test]
        public void MinuteEqualsReturnsTrueIfOtherIsSameReference()
        {
            c.Minute minute = new c.Minute(47);
            c.Minute other = minute;

            Assert.IsTrue(minute.Equals(other));
        }

        [Test]
        public void MinuteEqualsReturnsTrueIfOtherIsSameValue()
        {
            c.Minute minute = new c.Minute(47);
            c.Minute other = new c.Minute(47);

            Assert.IsTrue(minute.Equals(other));
        }

        [Test]
        public void MinuteEqualsReturnsFalseIfOtherIsDifferentValue()
        {
            c.Minute minute = new c.Minute(47);
            c.Minute other = new c.Minute(45);

            Assert.IsFalse(minute.Equals(other));
        }
        [Test]
        public void MinuteCompareToReturnsOneIfOtherIsNull()
        {
            const int expected = 1;

            c.Minute minute = new c.Minute(47);
            c.Minute other = null;

            Assert.AreEqual(expected, minute.CompareTo(other));
        }

        [Test]
        public void MinuteCompareToReturnsOneIfOtherIsNotTypeOfMinute()
        {
            const int expected = 1;

            c.Minute minute = new c.Minute(47);
            Task other = new Task(() => { });

            Assert.AreEqual(expected, minute.CompareTo(other));
        }

        [Test]
        public void MinuteCompareToReturnsOneIfOtherIsLessThanValue()
        {
            const int expected = 1;

            c.Minute minute = new c.Minute(47);
            c.Minute other = new c.Minute(46);

            Assert.AreEqual(expected, minute.CompareTo(other));
        }

        [Test]
        public void MinuteCompareToReturnsZeroIfOtherIsSameReference()
        {
            const int expected = 0;

            c.Minute minute = new c.Minute(47);
            c.Minute other = minute;

            Assert.AreEqual(expected, minute.CompareTo(other));
        }

        [Test]
        public void MinuteCompareToReturnsZeroIfOtherIsSameValue()
        {
            const int expected = 0;

            c.Minute minute = new c.Minute(47);
            c.Minute other = new c.Minute(47);

            Assert.AreEqual(expected, minute.CompareTo(other));
        }

        [Test]
        public void MinuteCompareToReturnsMinusOneIfOtherIsMoreThanValue()
        {
            const int expected = -1;

            c.Minute minute = new c.Minute(47);
            c.Minute other = new c.Minute(48);

            Assert.AreEqual(expected, minute.CompareTo(other));
        }
        [Test]
        public void MinuteEqualsOperatorReturnsTrueIfLeftAndRightAreSameValue()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = new c.Minute(47);

            Assert.IsTrue(left == right);
        }

        [Test]
        public void MinuteEqualsOperatorReturnsTrueIfLeftAndRightAreSameReference()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = left;

            Assert.IsTrue(left == right);
        }

        [Test]
        public void MinuteEqualsOperatorReturnsFalseIfLeftAndRightAreNotSameValue()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = new c.Minute(46);

            Assert.IsFalse(left == right);
        }

        [Test]
        public void MinuteEqualsOperatorReturnsTrueIfLeftAndRightAreNull()
        {
            c.Minute left = null;
            c.Minute right = null;

            Assert.IsTrue(left == right);
        }

        [Test]
        public void MinuteEqualsOperatorReturnsFalseIfLeftIsNullAndRightIsNotNull()
        {
            c.Minute left = null;
            c.Minute right = new c.Minute(47);

            Assert.IsFalse(left == right);
        }

        [Test]
        public void MinuteEqualsOperatorReturnsFalseIfLeftIsNotNullAndRightIsNull()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = null;

            Assert.IsFalse(left == right);
        }

        [Test]
        public void MinuteNotEqualsOperatorReturnsFalseIfLeftAndRightAreSameValue()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = new c.Minute(47);

            Assert.IsFalse(left != right);
        }

        [Test]
        public void MinuteNotEqualsOperatorReturnsFalseIfLeftAndRightAreSameReference()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = left;

            Assert.IsFalse(left != right);
        }

        [Test]
        public void MinuteNotEqualsOperatorReturnsTrueIfLeftAndRightAreNotSameValue()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = new c.Minute(46);

            Assert.IsTrue(left != right);
        }

        [Test]
        public void MinuteNotEqualsOperatorReturnsFalseIfLeftAndRightAreNull()
        {
            c.Minute left = null;
            c.Minute right = null;

            Assert.IsFalse(left != right);
        }

        [Test]
        public void MinuteNotEqualsOperatorReturnsTrueIfLeftIsNullAndRightIsNotNull()
        {
            c.Minute left = null;
            c.Minute right = new c.Minute(47);

            Assert.IsTrue(left != right);
        }

        [Test]
        public void MinuteNotEqualsOperatorReturnsTrueIfLeftIsNotNullAndRightIsNull()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = null;

            Assert.IsTrue(left != right);
        }

        [Test]
        public void MinuteLessThanOperatorReturnsFalseIfLeftIsGreaterThanRight()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = new c.Minute(46);

            Assert.IsFalse(left < right);
        }

        [Test]
        public void MinuteLessThanOperatorReturnsFalseIfLeftIsSameValueAsRight()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = new c.Minute(47);

            Assert.IsFalse(left < right);
        }

        [Test]
        public void MinuteLessThanOperatorReturnsFalseIfLeftIsSameReferenceAsRight()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = left;

            Assert.IsFalse(left < right);
        }

        [Test]
        public void MinuteLessThanOperatorReturnsTrueIfLeftIsLessThanRight()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = new c.Minute(48);

            Assert.IsTrue(left < right);
        }

        [Test]
        public void MinuteLessThanOperatorReturnsFalseIfLeftAndRightAreNull()
        {
            c.Minute left = null;
            c.Minute right = null;

            Assert.IsFalse(left < right);
        }

        [Test]
        public void MinuteLessThanOperatorReturnsTrueIfLeftIsNullAndRightIsNotNull()
        {
            c.Minute left = null;
            c.Minute right = new c.Minute(46);

            Assert.IsTrue(left < right);
        }

        [Test]
        public void MinuteLessThanOperatorReturnsFalseIfLeftIsNotNullAndRightIsNull()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = null;

            Assert.IsFalse(left < right);
        }

        [Test]
        public void MinuteGreaterThanOperatorReturnsTrueIfLeftIsGreaterThanRight()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = new c.Minute(46);

            Assert.IsTrue(left > right);
        }

        [Test]
        public void MinuteGreaterThanOperatorReturnsFalseIfLeftIsSameValueAsRight()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = new c.Minute(47);

            Assert.IsFalse(left > right);
        }

        [Test]
        public void MinuteGreaterThanOperatorReturnsFalseIfLeftIsSameReferenceAsRight()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = left;

            Assert.IsFalse(left > right);
        }

        [Test]
        public void MinuteGreaterThanOperatorReturnsFalseIfLeftIsLessThanRight()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = new c.Minute(48);

            Assert.IsFalse(left > right);
        }

        [Test]
        public void MinuteGreaterThanOperatorReturnsFalseIfLeftAndRightAreNull()
        {
            c.Minute left = null;
            c.Minute right = null;

            Assert.IsFalse(left > right);
        }

        [Test]
        public void MinuteGreaterThanOperatorReturnsFalseIfLeftIsNullAndRightIsNotNull()
        {
            c.Minute left = null;
            c.Minute right = new c.Minute(46);

            Assert.IsFalse(left > right);
        }

        [Test]
        public void MinuteGreaterThanOperatorReturnsTrueIfLeftIsNotNullAndRightIsNull()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = null;

            Assert.IsTrue(left > right);
        }

        [Test]
        public void MinuteLessThanOrEqualOperatorReturnsFalseIfLeftIsGreaterThanRight()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = new c.Minute(46);

            Assert.IsFalse(left <= right);
        }

        [Test]
        public void MinuteLessThanOrEqualOperatorReturnsTrueIfLeftIsSameValueAsRight()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = new c.Minute(47);

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void MinuteLessThanOrEqualOperatorReturnsTrueIfLeftIsSameReferenceAsRight()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = left;

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void MinuteLessThanOrEqualOperatorReturnsTrueIfLeftIsLessThanRight()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = new c.Minute(48);

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void MinuteLessThanOrEqualOperatorReturnsTrueIfLeftAndRightAreNull()
        {
            c.Minute left = null;
            c.Minute right = null;

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void MinuteLessThanOrEqualOperatorReturnsTrueIfLeftIsNullAndRightIsNotNull()
        {
            c.Minute left = null;
            c.Minute right = new c.Minute(46);

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void MinuteLessThanOrEqualOperatorReturnsFalseIfLeftIsNotNullAndRightIsNull()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = null;

            Assert.IsFalse(left <= right);
        }

        [Test]
        public void MinuteGreaterThanOrEqualOperatorReturnsTrueIfLeftIsGreaterThanRight()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = new c.Minute(46);

            Assert.IsTrue(left >= right);
        }

        [Test]
        public void MinuteGreaterThanOrEqualOperatorReturnsTrueIfLeftIsSameValueAsRight()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = new c.Minute(47);

            Assert.IsTrue(left >= right);
        }

        [Test]
        public void MinuteGreaterThanOrEqualOperatorReturnsTrueIfLeftIsSameReferenceAsRight()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = left;

            Assert.IsTrue(left >= right);
        }

        [Test]
        public void MinuteGreaterThanOrEqualOperatorReturnsFalseIfLeftIsLessThanRight()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = new c.Minute(48);

            Assert.IsFalse(left >= right);
        }

        [Test]
        public void MinuteGreaterThanOrEqualOperatorReturnsTrueIfLeftAndRightAreNull()
        {
            c.Minute left = null;
            c.Minute right = null;

            Assert.IsTrue(left >= right);
        }

        [Test]
        public void MinuteGreaterThanOrEqualOperatorReturnsFalseIfLeftIsNullAndRightIsNotNull()
        {
            c.Minute left = null;
            c.Minute right = new c.Minute(46);

            Assert.IsFalse(left >= right);
        }

        [Test]
        public void MinuteGreaterThanOrEqualOperatorReturnsTrueIfLeftIsNotNullAndRightIsNull()
        {
            c.Minute left = new c.Minute(47);
            c.Minute right = null;

            Assert.IsTrue(left >= right);
        }
    }
}
