using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace kx.Test.Connection
{
    [TestFixture]
    public class ConnectionNullTests
    {
        [Test]
        public void ConnectionGetsExpectedNullValueFromBooleanType()
        {
            const bool expected = false;

            var result = c.NULL(typeof(bool));

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetsExpectedNullValueFromGuidType()
        {
            Guid expected = default(Guid);

            var result = c.NULL(typeof(Guid));

            Assert.AreEqual(expected, result);

        }

        [Test]
        public void ConnectionGetsExpectedNullValueFromByteType()
        {
            const byte expected = 0;

            var result = c.NULL(typeof(byte));

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetsExpectedNullValueFromShortType()
        {
            const short expected = short.MinValue;

            var result = c.NULL(typeof(short));

            Assert.AreEqual(expected, result);

        }

        [Test]
        public void ConnectionGetsExpectedNullValueFromIntType()
        {
            const int expected = int.MinValue;

            var result = c.NULL(typeof(int));

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetsExpectedNullValueFromLongType()
        {
            const long expected = long.MinValue;

            var result = c.NULL(typeof(long));

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetsExpectedNullValueFromFloatType()
        {
            const float expected = (float)double.NaN;

            var result = c.NULL(typeof(float));

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetsExpectedNullValueFromDoubleType()
        {
            const double expected = double.NaN;

            var result = c.NULL(typeof(double));

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetsExpectedNullValueFromCharType()
        {
            const char expected = ' ';

            var result = c.NULL(typeof(char));

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetsExpectedNullValueFromStringType()
        {
            const string expected = "";

            var result = c.NULL(typeof(string));

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetsExpectedNullValueFromDateTimeType()
        {
            DateTime expected = new DateTime(0L);

            var result = c.NULL(typeof(DateTime));

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetsExpectedNullValueFromMonthType()
        {
            c.Month expected = new c.Month(int.MinValue);

            var result = c.NULL(typeof(c.Month));

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetsExpectedNullValueFromDateType()
        {
            c.Date expected = new c.Date(int.MinValue);

            var result = c.NULL(typeof(c.Date));

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetsExpectedNullValueFromKTimespanType()
        {
            c.KTimespan expected = new c.KTimespan(long.MinValue);

            var result = c.NULL(typeof(c.KTimespan));

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetsExpectedNullValueFromMinuteType()
        {
            c.Minute expected = new c.Minute(int.MinValue);

            var result = c.NULL(typeof(c.Minute));

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetsExpectedNullValueFromSecondType()
        {
            c.Second expected = new c.Second(int.MinValue);

            var result = c.NULL(typeof(c.Second));

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetsExpectedNullValueFromTimeSpanType()
        {
            TimeSpan expected = new TimeSpan(long.MinValue);

            var result = c.NULL(typeof(TimeSpan));

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetsExpectedNullValueFromCSharpSpecificType()
        {
            var result = c.NULL(typeof(Task));

            Assert.IsNull(result);
        }

        [Test]
        public void ConnectionGetsExpectedNullValueFromCSharpCustomType()
        {
            var result = c.NULL(typeof(TestType));

            Assert.IsNull(result);
        }

        [Test]
        public void ConnectionGetsExpectedNullValueForNullCharacterId()
        {
            var result = c.NULL(' ');

            Assert.IsNull(result);
        }

        [Test]
        public void ConnectionGetExpectedNullValueForBooleanCharacterId()
        {
            const bool expected = false;

            var result = c.NULL('b');

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetExpectedNullValueForGuidCharacterId()
        {
            Guid expected = default(Guid);

            var result = c.NULL('g');

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetExpectedNullValueForByteCharacterId()
        {
            const byte expected = 0;

            var result = c.NULL('x');

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetExpectedNullValueForShortCharacterId()
        {
            const short expected = short.MinValue;

            var result = c.NULL('h');

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetExpectedNullValueForIntCharacterId()
        {
            const int expected = int.MinValue;

            var result = c.NULL('i');

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetExpectedNullValueForLongCharacterId()
        {
            const long expected = long.MinValue;

            var result = c.NULL('j');

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetExpectedNullValueForFloatCharacterId()
        {
            const float expected = (float)double.NaN;

            var result = c.NULL('e');

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetExpectedNullValueForDoubleCharacterId()
        {
            const double expected = double.NaN;

            var result = c.NULL('f');

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetExpectedNullValueForCharCharacterId()
        {
            const char expected = ' ';

            var result = c.NULL('c');

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetExpectedNullValueForStringCharacterId()
        {
            const string expected = "";

            var result = c.NULL('s');

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetsExpectedNullValueForDateTimeCharacterId()
        {
            DateTime expected = new DateTime(0L);

            var result = c.NULL('p');

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetsExpectedNullValueForMonthCharacterId()
        {
            c.Month expected = new c.Month(int.MinValue);

            var result = c.NULL('m');

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetsExpectedNullValueForDateCharacterId()
        {
            c.Date expected = new c.Date(int.MinValue);

            var result = c.NULL('d');

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetsExpectedNullValueForKTimeSpanCharacterId()
        {
            c.KTimespan expected = new c.KTimespan(long.MinValue);

            var result = c.NULL('n');

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetsExpectedNullValueForMinuteCharacterId()
        {
            c.Minute expected = new c.Minute(int.MinValue);

            var result = c.NULL('u');

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetsExpectedNullValueForSecondCharacterId()
        {
            c.Second expected = new c.Second(int.MinValue);

            var result = c.NULL('v');

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionGetsExpectedNullValueForTimeSpanCharacterId()
        {
            TimeSpan expected = new TimeSpan(long.MinValue);

            var result = c.NULL('t');

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionNullThrowsIfUnrecognisedCharacterId()
        {
            Assert.Throws<ArgumentException>(() => c.NULL('@'));
        }

        [Test]
        public void ConnectionIsQNullReturnsFalseForBoolean()
        {
            bool input = false;

            Assert.IsFalse(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsTrueForGuid()
        {
            Guid input = default(Guid);

            Assert.IsTrue(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsFalseForByte()
        {
            byte input = 0;

            Assert.IsFalse(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsTrueForShort()
        {
            short input = short.MinValue;

            Assert.IsTrue(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsTrueForInt()
        {
            int input = int.MinValue;

            Assert.IsTrue(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsTrueForLong()
        {
            long input = long.MinValue;

            Assert.IsTrue(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsTrueForFloat()
        {
            float input = (float)double.NaN;

            Assert.IsTrue(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsTrueForDouble()
        {
            double input = double.NaN;

            Assert.IsTrue(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsTrueForChar()
        {
            char input = ' ';

            Assert.IsTrue(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsTrueForString()
        {
            string input = string.Empty;

            Assert.IsTrue(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsTrueForDateTime()
        {
            DateTime input = new DateTime(0L);

            Assert.IsTrue(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsTrueForMonth()
        {
            c.Month input = new c.Month(int.MinValue);

            Assert.IsTrue(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsTrueForDate()
        {
            c.Date input = new c.Date(int.MinValue);

            Assert.IsTrue(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsTrueForKTimespan()
        {
            c.KTimespan input = new c.KTimespan(long.MinValue);

            Assert.IsTrue(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsTrueForMinute()
        {
            c.Minute input = new c.Minute(int.MinValue);

            Assert.IsTrue(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsTrueForSecond()
        {
            c.Second input = new c.Second(int.MinValue);

            Assert.IsTrue(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsTrueForTimespan()
        {
            TimeSpan input = new TimeSpan(long.MinValue);

            Assert.IsTrue(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsFalseForBooleanArray()
        {
            bool[] input = Array.Empty<bool>();

            Assert.IsFalse(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsFalseForGuidArray()
        {
            Guid[] input = Array.Empty<Guid>();

            Assert.IsFalse(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsFalseForByteArray()
        {
            byte[] input = Array.Empty<byte>();

            Assert.IsFalse(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsFalseForShortArray()
        {
            short[] input = Array.Empty<short>();

            Assert.IsFalse(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsFalseForIntArray()
        {
            int[] input = Array.Empty<int>();

            Assert.IsFalse(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsFalseForLongArray()
        {
            long[] input = Array.Empty<long>();

            Assert.IsFalse(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsFalseForFloatArray()
        {
            float[] input = Array.Empty<float>();

            Assert.IsFalse(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsFalseForDoubleArray()
        {
            double[] input = Array.Empty<double>();

            Assert.IsFalse(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsFalseForCharArray()
        {
            char[] input = Array.Empty<char>();

            Assert.IsFalse(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsFalseForStringArray()
        {
            string[] input = Array.Empty<string>();

            Assert.IsFalse(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsFalseForDateTimeArray()
        {
            DateTime[] input = Array.Empty<DateTime>();

            Assert.IsFalse(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsFalseForDateArray()
        {
            c.Date[] input = Array.Empty<c.Date>();

            Assert.IsFalse(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsFalseForMinuteArray()
        {
            c.Minute[] input = Array.Empty<c.Minute>();

            Assert.IsFalse(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsFalseForSecondArray()
        {
            c.Second[] input = Array.Empty<c.Second>();

            Assert.IsFalse(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsFalseForKTimeSpanArray()
        {
            c.KTimespan[] input = Array.Empty<c.KTimespan>();

            Assert.IsFalse(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsFalseForTimeSpanArray()
        {
            TimeSpan[] input = Array.Empty<TimeSpan>();

            Assert.IsFalse(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsFalseForDict()
        {
            c.Dict input = new c.Dict(new string[] { }, new object[] { });

            Assert.IsFalse(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsFalseForFlip()
        {
            c.Flip input = new c.Flip(new c.Dict(new string[] { }, new object[] { }));

            Assert.IsFalse(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsFalseForCSharpSpecificType()
        {
            Task input = new Task(() => { });

            Assert.IsFalse(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsFalseForCSharpCustomType()
        {
            TestType input = new TestType();

            Assert.IsFalse(c.qn(input));
        }

        [Test]
        public void ConnectionIsQNullReturnsFalseForNull()
        {
            Assert.IsFalse(c.qn(null));
        }

        private class TestType { }
    }
}
