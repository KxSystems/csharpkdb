using System.Threading.Tasks;
using NUnit.Framework;

namespace kx.Test.Types
{
    [TestFixture]
    public class SecondTests
    {
        [Test]
        public void SecondInitialisesWithInt32()
        {
            c.Second second = new c.Second(1);

            Assert.IsNotNull(second);
        }

        [Test]
        public void SecondToStringReturnsExpectedString()
        {
            const string expected = "00:00:47";

            c.Second second = new c.Second(47);

            Assert.AreEqual(expected, second.ToString());
        }

        [Test]
        public void SecondToStringReturnsExpectedStringForMinInt32()
        {
            const string expected = "";

            c.Second second = new c.Second(int.MinValue);

            Assert.AreEqual(expected, second.ToString());
        }

        [Test]
        public void SecondGetHashCodeReturnsExpectedHash()
        {
            const int expected = 47;

            c.Second second = new c.Second(expected);

            Assert.AreEqual(expected, second.GetHashCode());
        }
        [Test]
        public void SecondEqualsReturnsFalseIfOtherIsNull()
        {
            c.Second second = new c.Second(47);
            c.Second other = null;

            Assert.IsFalse(second.Equals(other));
        }

        [Test]
        public void SecondEqualsReturnsFalseIfOtherIsNotTypeOfSecond()
        {
            c.Second second = new c.Second(47);
            Task other = new Task(() => { });

            Assert.IsFalse(second.Equals(other));
        }

        [Test]
        public void SecondEqualsReturnsTrueIfOtherIsSameReference()
        {
            c.Second second = new c.Second(47);
            c.Second other = second;

            Assert.IsTrue(second.Equals(other));
        }

        [Test]
        public void SecondEqualsReturnsTrueIfOtherIsSameValue()
        {
            c.Second second = new c.Second(47);
            c.Second other = new c.Second(47);

            Assert.IsTrue(second.Equals(other));
        }

        [Test]
        public void SecondEqualsReturnsFalseIfOtherIsDifferentValue()
        {
            c.Second second = new c.Second(47);
            c.Second other = new c.Second(45);

            Assert.IsFalse(second.Equals(other));
        }
        [Test]
        public void SecondCompareToReturnsOneIfOtherIsNull()
        {
            const int expected = 1;

            c.Second second = new c.Second(47);
            c.Second other = null;

            Assert.AreEqual(expected, second.CompareTo(other));
        }

        [Test]
        public void SecondCompareToReturnsOneIfOtherIsNotTypeOfSecond()
        {
            const int expected = 1;

            c.Second second = new c.Second(47);
            Task other = new Task(() => { });

            Assert.AreEqual(expected, second.CompareTo(other));
        }

        [Test]
        public void SecondCompareToReturnsOneIfOtherIsLessThanValue()
        {
            const int expected = 1;

            c.Second second = new c.Second(47);
            c.Second other = new c.Second(46);

            Assert.AreEqual(expected, second.CompareTo(other));
        }

        [Test]
        public void SecondCompareToReturnsZeroIfOtherIsSameReference()
        {
            const int expected = 0;

            c.Second second = new c.Second(47);
            c.Second other = second;

            Assert.AreEqual(expected, second.CompareTo(other));
        }

        [Test]
        public void SecondCompareToReturnsZeroIfOtherIsSameValue()
        {
            const int expected = 0;

            c.Second second = new c.Second(47);
            c.Second other = new c.Second(47);

            Assert.AreEqual(expected, second.CompareTo(other));
        }

        [Test]
        public void SecondCompareToReturnsMinusOneIfOtherIsMoreThanValue()
        {
            const int expected = -1;

            c.Second second = new c.Second(47);
            c.Second other = new c.Second(48);

            Assert.AreEqual(expected, second.CompareTo(other));
        }
        [Test]
        public void SecondEqualsOperatorReturnsTrueIfLeftAndRightAreSameValue()
        {
            c.Second left = new c.Second(47);
            c.Second right = new c.Second(47);

            Assert.IsTrue(left == right);
        }

        [Test]
        public void SecondEqualsOperatorReturnsTrueIfLeftAndRightAreSameReference()
        {
            c.Second left = new c.Second(47);
            c.Second right = left;

            Assert.IsTrue(left == right);
        }

        [Test]
        public void SecondEqualsOperatorReturnsFalseIfLeftAndRightAreNotSameValue()
        {
            c.Second left = new c.Second(47);
            c.Second right = new c.Second(46);

            Assert.IsFalse(left == right);
        }

        [Test]
        public void SecondEqualsOperatorReturnsTrueIfLeftAndRightAreNull()
        {
            c.Second left = null;
            c.Second right = null;

            Assert.IsTrue(left == right);
        }

        [Test]
        public void SecondEqualsOperatorReturnsFalseIfLeftIsNullAndRightIsNotNull()
        {
            c.Second left = null;
            c.Second right = new c.Second(47);

            Assert.IsFalse(left == right);
        }

        [Test]
        public void SecondEqualsOperatorReturnsFalseIfLeftIsNotNullAndRightIsNull()
        {
            c.Second left = new c.Second(47);
            c.Second right = null;

            Assert.IsFalse(left == right);
        }

        [Test]
        public void SecondNotEqualsOperatorReturnsFalseIfLeftAndRightAreSameValue()
        {
            c.Second left = new c.Second(47);
            c.Second right = new c.Second(47);

            Assert.IsFalse(left != right);
        }

        [Test]
        public void SecondNotEqualsOperatorReturnsFalseIfLeftAndRightAreSameReference()
        {
            c.Second left = new c.Second(47);
            c.Second right = left;

            Assert.IsFalse(left != right);
        }

        [Test]
        public void SecondNotEqualsOperatorReturnsTrueIfLeftAndRightAreNotSameValue()
        {
            c.Second left = new c.Second(47);
            c.Second right = new c.Second(46);

            Assert.IsTrue(left != right);
        }

        [Test]
        public void SecondNotEqualsOperatorReturnsFalseIfLeftAndRightAreNull()
        {
            c.Second left = null;
            c.Second right = null;

            Assert.IsFalse(left != right);
        }

        [Test]
        public void SecondNotEqualsOperatorReturnsTrueIfLeftIsNullAndRightIsNotNull()
        {
            c.Second left = null;
            c.Second right = new c.Second(47);

            Assert.IsTrue(left != right);
        }

        [Test]
        public void SecondNotEqualsOperatorReturnsTrueIfLeftIsNotNullAndRightIsNull()
        {
            c.Second left = new c.Second(47);
            c.Second right = null;

            Assert.IsTrue(left != right);
        }

        [Test]
        public void SecondLessThanOperatorReturnsFalseIfLeftIsGreaterThanRight()
        {
            c.Second left = new c.Second(47);
            c.Second right = new c.Second(46);

            Assert.IsFalse(left < right);
        }

        [Test]
        public void SecondLessThanOperatorReturnsFalseIfLeftIsSameValueAsRight()
        {
            c.Second left = new c.Second(47);
            c.Second right = new c.Second(47);

            Assert.IsFalse(left < right);
        }

        [Test]
        public void SecondLessThanOperatorReturnsFalseIfLeftIsSameReferenceAsRight()
        {
            c.Second left = new c.Second(47);
            c.Second right = left;

            Assert.IsFalse(left < right);
        }

        [Test]
        public void SecondLessThanOperatorReturnsTrueIfLeftIsLessThanRight()
        {
            c.Second left = new c.Second(47);
            c.Second right = new c.Second(48);

            Assert.IsTrue(left < right);
        }

        [Test]
        public void SecondLessThanOperatorReturnsFalseIfLeftAndRightAreNull()
        {
            c.Second left = null;
            c.Second right = null;

            Assert.IsFalse(left < right);
        }

        [Test]
        public void SecondLessThanOperatorReturnsTrueIfLeftIsNullAndRightIsNotNull()
        {
            c.Second left = null;
            c.Second right = new c.Second(46);

            Assert.IsTrue(left < right);
        }

        [Test]
        public void SecondLessThanOperatorReturnsFalseIfLeftIsNotNullAndRightIsNull()
        {
            c.Second left = new c.Second(47);
            c.Second right = null;

            Assert.IsFalse(left < right);
        }

        [Test]
        public void SecondGreaterThanOperatorReturnsTrueIfLeftIsGreaterThanRight()
        {
            c.Second left = new c.Second(47);
            c.Second right = new c.Second(46);

            Assert.IsTrue(left > right);
        }

        [Test]
        public void SecondGreaterThanOperatorReturnsFalseIfLeftIsSameValueAsRight()
        {
            c.Second left = new c.Second(47);
            c.Second right = new c.Second(47);

            Assert.IsFalse(left > right);
        }

        [Test]
        public void SecondGreaterThanOperatorReturnsFalseIfLeftIsSameReferenceAsRight()
        {
            c.Second left = new c.Second(47);
            c.Second right = left;

            Assert.IsFalse(left > right);
        }

        [Test]
        public void SecondGreaterThanOperatorReturnsFalseIfLeftIsLessThanRight()
        {
            c.Second left = new c.Second(47);
            c.Second right = new c.Second(48);

            Assert.IsFalse(left > right);
        }

        [Test]
        public void SecondGreaterThanOperatorReturnsFalseIfLeftAndRightAreNull()
        {
            c.Second left = null;
            c.Second right = null;

            Assert.IsFalse(left > right);
        }

        [Test]
        public void SecondGreaterThanOperatorReturnsFalseIfLeftIsNullAndRightIsNotNull()
        {
            c.Second left = null;
            c.Second right = new c.Second(46);

            Assert.IsFalse(left > right);
        }

        [Test]
        public void SecondGreaterThanOperatorReturnsTrueIfLeftIsNotNullAndRightIsNull()
        {
            c.Second left = new c.Second(47);
            c.Second right = null;

            Assert.IsTrue(left > right);
        }

        [Test]
        public void SecondLessThanOrEqualOperatorReturnsFalseIfLeftIsGreaterThanRight()
        {
            c.Second left = new c.Second(47);
            c.Second right = new c.Second(46);

            Assert.IsFalse(left <= right);
        }

        [Test]
        public void SecondLessThanOrEqualOperatorReturnsTrueIfLeftIsSameValueAsRight()
        {
            c.Second left = new c.Second(47);
            c.Second right = new c.Second(47);

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void SecondLessThanOrEqualOperatorReturnsTrueIfLeftIsSameReferenceAsRight()
        {
            c.Second left = new c.Second(47);
            c.Second right = left;

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void SecondLessThanOrEqualOperatorReturnsTrueIfLeftIsLessThanRight()
        {
            c.Second left = new c.Second(47);
            c.Second right = new c.Second(48);

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void SecondLessThanOrEqualOperatorReturnsTrueIfLeftAndRightAreNull()
        {
            c.Second left = null;
            c.Second right = null;

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void SecondLessThanOrEqualOperatorReturnsTrueIfLeftIsNullAndRightIsNotNull()
        {
            c.Second left = null;
            c.Second right = new c.Second(46);

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void SecondLessThanOrEqualOperatorReturnsFalseIfLeftIsNotNullAndRightIsNull()
        {
            c.Second left = new c.Second(47);
            c.Second right = null;

            Assert.IsFalse(left <= right);
        }

        [Test]
        public void SecondGreaterThanOrEqualOperatorReturnsTrueIfLeftIsGreaterThanRight()
        {
            c.Second left = new c.Second(47);
            c.Second right = new c.Second(46);

            Assert.IsTrue(left >= right);
        }

        [Test]
        public void SecondGreaterThanOrEqualOperatorReturnsTrueIfLeftIsSameValueAsRight()
        {
            c.Second left = new c.Second(47);
            c.Second right = new c.Second(47);

            Assert.IsTrue(left >= right);
        }

        [Test]
        public void SecondGreaterThanOrEqualOperatorReturnsTrueIfLeftIsSameReferenceAsRight()
        {
            c.Second left = new c.Second(47);
            c.Second right = left;

            Assert.IsTrue(left >= right);
        }

        [Test]
        public void SecondGreaterThanOrEqualOperatorReturnsFalseIfLeftIsLessThanRight()
        {
            c.Second left = new c.Second(47);
            c.Second right = new c.Second(48);

            Assert.IsFalse(left >= right);
        }

        [Test]
        public void SecondGreaterThanOrEqualOperatorReturnsTrueIfLeftAndRightAreNull()
        {
            c.Second left = null;
            c.Second right = null;

            Assert.IsTrue(left >= right);
        }

        [Test]
        public void SecondGreaterThanOrEqualOperatorReturnsFalseIfLeftIsNullAndRightIsNotNull()
        {
            c.Second left = null;
            c.Second right = new c.Second(46);

            Assert.IsFalse(left >= right);
        }

        [Test]
        public void SecondGreaterThanOrEqualOperatorReturnsTrueIfLeftIsNotNullAndRightIsNull()
        {
            c.Second left = new c.Second(47);
            c.Second right = null;

            Assert.IsTrue(left >= right);
        }
    }
}
