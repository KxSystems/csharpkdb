using System;
using System.Globalization;
using System.Threading.Tasks;
using NUnit.Framework;

namespace kx.Test.Types
{
    [TestFixture]
    public class DateTests
    {
        [Test]
        public void DateInitialisesWithInt32()
        {
            c.Date date = new c.Date(1);

            Assert.IsNotNull(date);
        }

        [Test]
        public void DateInitialisesWithInt64()
        {
            c.Date date = new c.Date(1L);

            Assert.IsNotNull(date);
        }

        [Test]
        public void DateInitialisesWithZeroInt64()
        {
            c.Date date = new c.Date(0L);

            Assert.IsNotNull(date);
        }

        [Test]
        public void DateInitialisesWithDateTime()
        {
            c.Date date = new c.Date(DateTime.UtcNow);

            Assert.IsNotNull(date);
        }

        [Test]
        public void DateToDateTimeReturnsExpectedDateTime()
        {
            DateTime expected = new DateTime(2020, 11, 04, 0, 0, 0, DateTimeKind.Utc);

            c.Date date = new c.Date(expected);

            Assert.AreEqual(expected, date.DateTime());
        }

        [Test]
        public void DateToDateTimeReturnsExpectedDateTimeIfValueIsInt32Min()
        {
            DateTime expected = DateTime.MinValue.AddTicks(1L);

            c.Date date = new c.Date(int.MinValue + 1);

            Assert.AreEqual(expected, date.DateTime());
        }

        [Test]
        public void DateToDateTimeReturnsExpectedDateTimeIfValueIsInt32Max()
        {
            DateTime expected = DateTime.MaxValue;

            c.Date date = new c.Date(int.MaxValue);

            Assert.AreEqual(expected, date.DateTime());
        }

        [Test]
        public void DateToStringReturnsExpectedString()
        {
            string expected = new DateTime(2020, 11, 04, 0, 0, 0, DateTimeKind.Utc)
                .ToString("d", CultureInfo.InvariantCulture);

            c.Date date = new c.Date(new DateTime(2020, 11, 04, 0, 0, 0, DateTimeKind.Utc));

            string result = date.ToString();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void DateToStringReturnsExpectedStringForMinInt32()
        {
            string expected = string.Empty;

            c.Date date = new c.Date(int.MinValue);

            string result = date.ToString();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void DateToStringReturnsExpectedStringForMaxInt32()
        {
            string expected = DateTime.MaxValue
                .ToString("d", CultureInfo.InvariantCulture);

            c.Date date = new c.Date(int.MaxValue);

            string result = date.ToString();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void DateGetHashCodeReturnsExpectedHash()
        {
            const int expected = 47;

            c.Date date = new c.Date(expected);

            Assert.AreEqual(expected, date.GetHashCode());
        }

        [Test]
        public void DateEqualsReturnsFalseIfOtherIsNull()
        {
            c.Date date = new c.Date(47);
            c.Date other = null;

            Assert.IsFalse(date.Equals(other));
        }

        [Test]
        public void DateEqualsReturnsFalseIfOtherIsNotTypeOfDate()
        {
            c.Date date = new c.Date(47);
            Task other = new Task(() => { });

            Assert.IsFalse(date.Equals(other));
        }

        [Test]
        public void DateEqualsReturnsTrueIfOtherIsSameReference()
        {
            c.Date date = new c.Date(47);
            c.Date other = date;

            Assert.IsTrue(date.Equals(other));
        }

        [Test]
        public void DateEqualsReturnsTrueIfOtherIsSameValue()
        {
            c.Date date = new c.Date(47);
            c.Date other = new c.Date(47);

            Assert.IsTrue(date.Equals(other));
        }

        [Test]
        public void DateEqualsReturnsFalseIfOtherIsDifferentValue()
        {
            c.Date date = new c.Date(47);
            c.Date other = new c.Date(45);

            Assert.IsFalse(date.Equals(other));
        }

        [Test]
        public void DateCompareToReturnsOneIfOtherIsNull()
        {
            const int expected = 1;

            c.Date date = new c.Date(47);
            c.Date other = null;

            Assert.AreEqual(expected, date.CompareTo(other));
        }

        [Test]
        public void DateCompareToReturnsOneIfOtherIsNotTypeOfDate()
        {
            const int expected = 1;

            c.Date date = new c.Date(47);
            Task other = new Task(() => { });

            Assert.AreEqual(expected, date.CompareTo(other));
        }

        [Test]
        public void DateCompareToReturnsOneIfOtherIsLessThanValue()
        {
            const int expected = 1;

            c.Date date = new c.Date(47);
            c.Date other = new c.Date(46);

            Assert.AreEqual(expected, date.CompareTo(other));
        }

        [Test]
        public void DateCompareToReturnsZeroIfOtherIsSameReference()
        {
            const int expected = 0;

            c.Date date = new c.Date(47);
            c.Date other = date;

            Assert.AreEqual(expected, date.CompareTo(other));
        }

        [Test]
        public void DateCompareToReturnsZeroIfOtherIsSameValue()
        {
            const int expected = 0;

            c.Date date = new c.Date(47);
            c.Date other = new c.Date(47);

            Assert.AreEqual(expected, date.CompareTo(other));
        }

        [Test]
        public void DateCompareToReturnsMinusOneIfOtherIsMoreThanValue()
        {
            const int expected = -1;

            c.Date date = new c.Date(47);
            c.Date other = new c.Date(48);

            Assert.AreEqual(expected, date.CompareTo(other));
        }

        [Test]
        public void DateEqualsOperatorReturnsTrueIfLeftAndRightAreSameValue()
        {
            c.Date left = new c.Date(47);
            c.Date right = new c.Date(47);

            Assert.IsTrue(left == right);
        }

        [Test]
        public void DateEqualsOperatorReturnsTrueIfLeftAndRightAreSameReference()
        {
            c.Date left = new c.Date(47);
            c.Date right = left;

            Assert.IsTrue(left == right);
        }

        [Test]
        public void DateEqualsOperatorReturnsFalseIfLeftAndRightAreNotSameValue()
        {
            c.Date left = new c.Date(47);
            c.Date right = new c.Date(46);

            Assert.IsFalse(left == right);
        }

        [Test]
        public void DateEqualsOperatorReturnsTrueIfLeftAndRightAreNull()
        {
            c.Date left = null;
            c.Date right = null;

            Assert.IsTrue(left == right);
        }

        [Test]
        public void DateEqualsOperatorReturnsFalseIfLeftIsNullAndRightIsNotNull()
        {
            c.Date left = null;
            c.Date right = new c.Date(47);

            Assert.IsFalse(left == right);
        }

        [Test]
        public void DateEqualsOperatorReturnsFalseIfLeftIsNotNullAndRightIsNull()
        {
            c.Date left = new c.Date(47);
            c.Date right = null;

            Assert.IsFalse(left == right);
        }

        [Test]
        public void DateNotEqualsOperatorReturnsFalseIfLeftAndRightAreSameValue()
        {
            c.Date left = new c.Date(47);
            c.Date right = new c.Date(47);

            Assert.IsFalse(left != right);
        }

        [Test]
        public void DateNotEqualsOperatorReturnsFalseIfLeftAndRightAreSameReference()
        {
            c.Date left = new c.Date(47);
            c.Date right = left;

            Assert.IsFalse(left != right);
        }

        [Test]
        public void DateNotEqualsOperatorReturnsTrueIfLeftAndRightAreNotSameValue()
        {
            c.Date left = new c.Date(47);
            c.Date right = new c.Date(46);

            Assert.IsTrue(left != right);
        }

        [Test]
        public void DateNotEqualsOperatorReturnsFalseIfLeftAndRightAreNull()
        {
            c.Date left = null;
            c.Date right = null;

            Assert.IsFalse(left != right);
        }

        [Test]
        public void DateNotEqualsOperatorReturnsTrueIfLeftIsNullAndRightIsNotNull()
        {
            c.Date left = null;
            c.Date right = new c.Date(47);

            Assert.IsTrue(left != right);
        }

        [Test]
        public void DateNotEqualsOperatorReturnsTrueIfLeftIsNotNullAndRightIsNull()
        {
            c.Date left = new c.Date(47);
            c.Date right = null;

            Assert.IsTrue(left != right);
        }

        [Test]
        public void DateLessThanOperatorReturnsFalseIfLeftIsGreaterThanRight()
        {
            c.Date left = new c.Date(47);
            c.Date right = new c.Date(46);

            Assert.IsFalse(left < right);
        }

        [Test]
        public void DateLessThanOperatorReturnsFalseIfLeftIsSameValueAsRight()
        {
            c.Date left = new c.Date(47);
            c.Date right = new c.Date(47);

            Assert.IsFalse(left < right);
        }

        [Test]
        public void DateLessThanOperatorReturnsFalseIfLeftIsSameReferenceAsRight()
        {
            c.Date left = new c.Date(47);
            c.Date right = left;

            Assert.IsFalse(left < right);
        }

        [Test]
        public void DateLessThanOperatorReturnsTrueIfLeftIsLessThanRight()
        {
            c.Date left = new c.Date(47);
            c.Date right = new c.Date(48);

            Assert.IsTrue(left < right);
        }

        [Test]
        public void DateLessThanOperatorReturnsFalseIfLeftAndRightAreNull()
        {
            c.Date left = null;
            c.Date right = null;

            Assert.IsFalse(left < right);
        }

        [Test]
        public void DateLessThanOperatorReturnsTrueIfLeftIsNullAndRightIsNotNull()
        {
            c.Date left = null;
            c.Date right = new c.Date(46);

            Assert.IsTrue(left < right);
        }

        [Test]
        public void DateLessThanOperatorReturnsFalseIfLeftIsNotNullAndRightIsNull()
        {
            c.Date left = new c.Date(47);
            c.Date right = null;

            Assert.IsFalse(left < right);
        }

        [Test]
        public void DateGreaterThanOperatorReturnsTrueIfLeftIsGreaterThanRight()
        {
            c.Date left = new c.Date(47);
            c.Date right = new c.Date(46);

            Assert.IsTrue(left > right);
        }

        [Test]
        public void DateGreaterThanOperatorReturnsFalseIfLeftIsSameValueAsRight()
        {
            c.Date left = new c.Date(47);
            c.Date right = new c.Date(47);

            Assert.IsFalse(left > right);
        }

        [Test]
        public void DateGreaterThanOperatorReturnsFalseIfLeftIsSameReferenceAsRight()
        {
            c.Date left = new c.Date(47);
            c.Date right = left;

            Assert.IsFalse(left > right);
        }

        [Test]
        public void DateGreaterThanOperatorReturnsFalseIfLeftIsLessThanRight()
        {
            c.Date left = new c.Date(47);
            c.Date right = new c.Date(48);

            Assert.IsFalse(left > right);
        }

        [Test]
        public void DateGreaterThanOperatorReturnsFalseIfLeftAndRightAreNull()
        {
            c.Date left = null;
            c.Date right = null;

            Assert.IsFalse(left > right);
        }

        [Test]
        public void DateGreaterThanOperatorReturnsFalseIfLeftIsNullAndRightIsNotNull()
        {
            c.Date left = null;
            c.Date right = new c.Date(46);

            Assert.IsFalse(left > right);
        }

        [Test]
        public void DateGreaterThanOperatorReturnsTrueIfLeftIsNotNullAndRightIsNull()
        {
            c.Date left = new c.Date(47);
            c.Date right = null;

            Assert.IsTrue(left > right);
        }

        [Test]
        public void DateLessThanOrEqualOperatorReturnsFalseIfLeftIsGreaterThanRight()
        {
            c.Date left = new c.Date(47);
            c.Date right = new c.Date(46);

            Assert.IsFalse(left <= right);
        }

        [Test]
        public void DateLessThanOrEqualOperatorReturnsTrueIfLeftIsSameValueAsRight()
        {
            c.Date left = new c.Date(47);
            c.Date right = new c.Date(47);

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void DateLessThanOrEqualOperatorReturnsTrueIfLeftIsSameReferenceAsRight()
        {
            c.Date left = new c.Date(47);
            c.Date right = left;

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void DateLessThanOrEqualOperatorReturnsTrueIfLeftIsLessThanRight()
        {
            c.Date left = new c.Date(47);
            c.Date right = new c.Date(48);

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void DateLessThanOrEqualOperatorReturnsTrueIfLeftAndRightAreNull()
        {
            c.Date left = null;
            c.Date right = null;

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void DateLessThanOrEqualOperatorReturnsTrueIfLeftIsNullAndRightIsNotNull()
        {
            c.Date left = null;
            c.Date right = new c.Date(46);

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void DateLessThanOrEqualOperatorReturnsFalseIfLeftIsNotNullAndRightIsNull()
        {
            c.Date left = new c.Date(47);
            c.Date right = null;

            Assert.IsFalse(left <= right);
        }

        [Test]
        public void DateGreaterThanOrEqualOperatorReturnsTrueIfLeftIsGreaterThanRight()
        {
            c.Date left = new c.Date(47);
            c.Date right = new c.Date(46);

            Assert.IsTrue(left >= right);
        }

        [Test]
        public void DateGreaterThanOrEqualOperatorReturnsTrueIfLeftIsSameValueAsRight()
        {
            c.Date left = new c.Date(47);
            c.Date right = new c.Date(47);

            Assert.IsTrue(left >= right);
        }

        [Test]
        public void DateGreaterThanOrEqualOperatorReturnsTrueIfLeftIsSameReferenceAsRight()
        {
            c.Date left = new c.Date(47);
            c.Date right = left;

            Assert.IsTrue(left >= right);
        }

        [Test]
        public void DateGreaterThanOrEqualOperatorReturnsFalseIfLeftIsLessThanRight()
        {
            c.Date left = new c.Date(47);
            c.Date right = new c.Date(48);

            Assert.IsFalse(left >= right);
        }

        [Test]
        public void DateGreaterThanOrEqualOperatorReturnsTrueIfLeftAndRightAreNull()
        {
            c.Date left = null;
            c.Date right = null;

            Assert.IsTrue(left >= right);
        }

        [Test]
        public void DateGreaterThanOrEqualOperatorReturnsFalseIfLeftIsNullAndRightIsNotNull()
        {
            c.Date left = null;
            c.Date right = new c.Date(46);

            Assert.IsFalse(left >= right);
        }

        [Test]
        public void DateGreaterThanOrEqualOperatorReturnsTrueIfLeftIsNotNullAndRightIsNull()
        {
            c.Date left = new c.Date(47);
            c.Date right = null;

            Assert.IsTrue(left >= right);
        }
    }
}
