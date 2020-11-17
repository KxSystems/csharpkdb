using System.Threading.Tasks;
using NUnit.Framework;

namespace kx.Test.Types
{
    [TestFixture]
    public class KTimespanTests
    {
        [Test]
        public void KTimespanInitialisesWithInt64()
        {
            c.KTimespan kTimespan = new c.KTimespan(1);

            Assert.IsNotNull(kTimespan);
        }

        [Test]
        public void KTimespanToStringReturnsExpectedString()
        {
            const string expected = "00:00:00.0000047";

            c.KTimespan kTimespan = new c.KTimespan(4700);

            Assert.AreEqual(expected, kTimespan.ToString());
        }

        [Test]
        public void KTimespanToStringReturnsExpectedStringForMinInt64()
        {
            const string expected = "";

            c.KTimespan kTimespan = new c.KTimespan(long.MinValue);

            Assert.AreEqual(expected, kTimespan.ToString());
        }

        [Test]
        public void KTimespanGetHashCodeReturnsExpectedHash()
        {
            const int expected = 47;

            c.KTimespan kTimespan = new c.KTimespan(4700);

            Assert.AreEqual(expected, kTimespan.GetHashCode());
        }

        [Test]
        public void KTimespanEqualsReturnsFalseIfOtherIsNull()
        {
            c.KTimespan kTimespan = new c.KTimespan(4700);
            c.KTimespan other = null;

            Assert.IsFalse(kTimespan.Equals(other));
        }

        [Test]
        public void KTimespanEqualsReturnsFalseIfOtherIsNotTypeOfKTimespan()
        {
            c.KTimespan kTimespan = new c.KTimespan(4700);
            Task other = new Task(() => { });

            Assert.IsFalse(kTimespan.Equals(other));
        }

        [Test]
        public void KTimespanEqualsReturnsTrueIfOtherIsSameReference()
        {
            c.KTimespan kTimespan = new c.KTimespan(4700);
            c.KTimespan other = kTimespan;

            Assert.IsTrue(kTimespan.Equals(other));
        }

        [Test]
        public void KTimespanEqualsReturnsTrueIfOtherIsSameValue()
        {
            c.KTimespan kTimespan = new c.KTimespan(4700);
            c.KTimespan other = new c.KTimespan(4700);

            Assert.IsTrue(kTimespan.Equals(other));
        }

        [Test]
        public void KTimespanEqualsReturnsFalseIfOtherIsDifferentValue()
        {
            c.KTimespan kTimespan = new c.KTimespan(4700);
            c.KTimespan other = new c.KTimespan(4500);

            Assert.IsFalse(kTimespan.Equals(other));
        }
        [Test]
        public void KTimespanCompareToReturnsOneIfOtherIsNull()
        {
            const int expected = 1;

            c.KTimespan kTimespan = new c.KTimespan(4700);
            c.KTimespan other = null;

            Assert.AreEqual(expected, kTimespan.CompareTo(other));
        }

        [Test]
        public void KTimespanCompareToReturnsOneIfOtherIsNotTypeOfKTimespan()
        {
            const int expected = 1;

            c.KTimespan kTimespan = new c.KTimespan(4700);
            Task other = new Task(() => { });

            Assert.AreEqual(expected, kTimespan.CompareTo(other));
        }

        [Test]
        public void KTimespanCompareToReturnsOneIfOtherIsLessThanValue()
        {
            const int expected = 1;

            c.KTimespan kTimespan = new c.KTimespan(4700);
            c.KTimespan other = new c.KTimespan(4600);

            Assert.AreEqual(expected, kTimespan.CompareTo(other));
        }

        [Test]
        public void KTimespanCompareToReturnsZeroIfOtherIsSameReference()
        {
            const int expected = 0;

            c.KTimespan kTimespan = new c.KTimespan(4700);
            c.KTimespan other = kTimespan;

            Assert.AreEqual(expected, kTimespan.CompareTo(other));
        }

        [Test]
        public void KTimespanCompareToReturnsZeroIfOtherIsSameValue()
        {
            const int expected = 0;

            c.KTimespan kTimespan = new c.KTimespan(4700);
            c.KTimespan other = new c.KTimespan(4700);

            Assert.AreEqual(expected, kTimespan.CompareTo(other));
        }

        [Test]
        public void KTimespanCompareToReturnsMinusOneIfOtherIsMoreThanValue()
        {
            const int expected = -1;

            c.KTimespan kTimespan = new c.KTimespan(4700);
            c.KTimespan other = new c.KTimespan(4800);

            Assert.AreEqual(expected, kTimespan.CompareTo(other));
        }
        [Test]
        public void KTimespanEqualsOperatorReturnsTrueIfLeftAndRightAreSameValue()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = new c.KTimespan(4700);

            Assert.IsTrue(left == right);
        }

        [Test]
        public void KTimespanEqualsOperatorReturnsTrueIfLeftAndRightAreSameReference()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = left;

            Assert.IsTrue(left == right);
        }

        [Test]
        public void KTimespanEqualsOperatorReturnsFalseIfLeftAndRightAreNotSameValue()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = new c.KTimespan(4600);

            Assert.IsFalse(left == right);
        }

        [Test]
        public void KTimespanEqualsOperatorReturnsTrueIfLeftAndRightAreNull()
        {
            c.KTimespan left = null;
            c.KTimespan right = null;

            Assert.IsTrue(left == right);
        }

        [Test]
        public void KTimespanEqualsOperatorReturnsFalseIfLeftIsNullAndRightIsNotNull()
        {
            c.KTimespan left = null;
            c.KTimespan right = new c.KTimespan(4700);

            Assert.IsFalse(left == right);
        }

        [Test]
        public void KTimespanEqualsOperatorReturnsFalseIfLeftIsNotNullAndRightIsNull()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = null;

            Assert.IsFalse(left == right);
        }

        [Test]
        public void KTimespanNotEqualsOperatorReturnsFalseIfLeftAndRightAreSameValue()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = new c.KTimespan(4700);

            Assert.IsFalse(left != right);
        }

        [Test]
        public void KTimespanNotEqualsOperatorReturnsFalseIfLeftAndRightAreSameReference()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = left;

            Assert.IsFalse(left != right);
        }

        [Test]
        public void KTimespanNotEqualsOperatorReturnsTrueIfLeftAndRightAreNotSameValue()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = new c.KTimespan(4600);

            Assert.IsTrue(left != right);
        }

        [Test]
        public void KTimespanNotEqualsOperatorReturnsFalseIfLeftAndRightAreNull()
        {
            c.KTimespan left = null;
            c.KTimespan right = null;

            Assert.IsFalse(left != right);
        }

        [Test]
        public void KTimespanNotEqualsOperatorReturnsTrueIfLeftIsNullAndRightIsNotNull()
        {
            c.KTimespan left = null;
            c.KTimespan right = new c.KTimespan(4700);

            Assert.IsTrue(left != right);
        }

        [Test]
        public void KTimespanNotEqualsOperatorReturnsTrueIfLeftIsNotNullAndRightIsNull()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = null;

            Assert.IsTrue(left != right);
        }

        [Test]
        public void KTimespanLessThanOperatorReturnsFalseIfLeftIsGreaterThanRight()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = new c.KTimespan(4600);

            Assert.IsFalse(left < right);
        }

        [Test]
        public void KTimespanLessThanOperatorReturnsFalseIfLeftIsSameValueAsRight()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = new c.KTimespan(4700);

            Assert.IsFalse(left < right);
        }

        [Test]
        public void KTimespanLessThanOperatorReturnsFalseIfLeftIsSameReferenceAsRight()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = left;

            Assert.IsFalse(left < right);
        }

        [Test]
        public void KTimespanLessThanOperatorReturnsTrueIfLeftIsLessThanRight()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = new c.KTimespan(4800);

            Assert.IsTrue(left < right);
        }

        [Test]
        public void KTimespanLessThanOperatorReturnsFalseIfLeftAndRightAreNull()
        {
            c.KTimespan left = null;
            c.KTimespan right = null;

            Assert.IsFalse(left < right);
        }

        [Test]
        public void KTimespanLessThanOperatorReturnsTrueIfLeftIsNullAndRightIsNotNull()
        {
            c.KTimespan left = null;
            c.KTimespan right = new c.KTimespan(4600);

            Assert.IsTrue(left < right);
        }

        [Test]
        public void KTimespanLessThanOperatorReturnsFalseIfLeftIsNotNullAndRightIsNull()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = null;

            Assert.IsFalse(left < right);
        }

        [Test]
        public void KTimespanGreaterThanOperatorReturnsTrueIfLeftIsGreaterThanRight()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = new c.KTimespan(4600);

            Assert.IsTrue(left > right);
        }

        [Test]
        public void KTimespanGreaterThanOperatorReturnsFalseIfLeftIsSameValueAsRight()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = new c.KTimespan(4700);

            Assert.IsFalse(left > right);
        }

        [Test]
        public void KTimespanGreaterThanOperatorReturnsFalseIfLeftIsSameReferenceAsRight()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = left;

            Assert.IsFalse(left > right);
        }

        [Test]
        public void KTimespanGreaterThanOperatorReturnsFalseIfLeftIsLessThanRight()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = new c.KTimespan(4800);

            Assert.IsFalse(left > right);
        }

        [Test]
        public void KTimespanGreaterThanOperatorReturnsFalseIfLeftAndRightAreNull()
        {
            c.KTimespan left = null;
            c.KTimespan right = null;

            Assert.IsFalse(left > right);
        }

        [Test]
        public void KTimespanGreaterThanOperatorReturnsFalseIfLeftIsNullAndRightIsNotNull()
        {
            c.KTimespan left = null;
            c.KTimespan right = new c.KTimespan(4600);

            Assert.IsFalse(left > right);
        }

        [Test]
        public void KTimespanGreaterThanOperatorReturnsTrueIfLeftIsNotNullAndRightIsNull()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = null;

            Assert.IsTrue(left > right);
        }

        [Test]
        public void KTimespanLessThanOrEqualOperatorReturnsFalseIfLeftIsGreaterThanRight()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = new c.KTimespan(4600);

            Assert.IsFalse(left <= right);
        }

        [Test]
        public void KTimespanLessThanOrEqualOperatorReturnsTrueIfLeftIsSameValueAsRight()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = new c.KTimespan(4700);

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void KTimespanLessThanOrEqualOperatorReturnsTrueIfLeftIsSameReferenceAsRight()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = left;

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void KTimespanLessThanOrEqualOperatorReturnsTrueIfLeftIsLessThanRight()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = new c.KTimespan(4800);

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void KTimespanLessThanOrEqualOperatorReturnsTrueIfLeftAndRightAreNull()
        {
            c.KTimespan left = null;
            c.KTimespan right = null;

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void KTimespanLessThanOrEqualOperatorReturnsTrueIfLeftIsNullAndRightIsNotNull()
        {
            c.KTimespan left = null;
            c.KTimespan right = new c.KTimespan(4600);

            Assert.IsTrue(left <= right);
        }

        [Test]
        public void KTimespanLessThanOrEqualOperatorReturnsFalseIfLeftIsNotNullAndRightIsNull()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = null;

            Assert.IsFalse(left <= right);
        }

        [Test]
        public void KTimespanGreaterThanOrEqualOperatorReturnsTrueIfLeftIsGreaterThanRight()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = new c.KTimespan(4600);

            Assert.IsTrue(left >= right);
        }

        [Test]
        public void KTimespanGreaterThanOrEqualOperatorReturnsTrueIfLeftIsSameValueAsRight()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = new c.KTimespan(4700);

            Assert.IsTrue(left >= right);
        }

        [Test]
        public void KTimespanGreaterThanOrEqualOperatorReturnsTrueIfLeftIsSameReferenceAsRight()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = left;

            Assert.IsTrue(left >= right);
        }

        [Test]
        public void KTimespanGreaterThanOrEqualOperatorReturnsFalseIfLeftIsLessThanRight()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = new c.KTimespan(4800);

            Assert.IsFalse(left >= right);
        }

        [Test]
        public void KTimespanGreaterThanOrEqualOperatorReturnsTrueIfLeftAndRightAreNull()
        {
            c.KTimespan left = null;
            c.KTimespan right = null;

            Assert.IsTrue(left >= right);
        }

        [Test]
        public void KTimespanGreaterThanOrEqualOperatorReturnsFalseIfLeftIsNullAndRightIsNotNull()
        {
            c.KTimespan left = null;
            c.KTimespan right = new c.KTimespan(4600);

            Assert.IsFalse(left >= right);
        }

        [Test]
        public void KTimespanGreaterThanOrEqualOperatorReturnsTrueIfLeftIsNotNullAndRightIsNull()
        {
            c.KTimespan left = new c.KTimespan(4700);
            c.KTimespan right = null;

            Assert.IsTrue(left >= right);
        }
    }
}
