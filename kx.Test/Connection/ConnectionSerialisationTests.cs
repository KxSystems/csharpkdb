using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace kx.Test.Connection
{
    [TestFixture]
    public class ConnectionSerialisationTests
    {
        private readonly int _testVersionNumber = 3;

        [Test]
        public void ConnectionSerialiseThrowsIfInputIsNull()
        {
            using (var connection = new c(_testVersionNumber))
            {
                Assert.Throws<ArgumentNullException>(() => connection.Serialize(1, null));
            }
        }

        [Test]
        public void ConnectionSerialiseThrowsKExceptionWithInnerExceptionIfSerialisationThrowsException()
        {
            using (var connection = new c(_testVersionNumber))
            {

                c.Dict dataRow = new c.Dict(
                new[]
                {
                "sym"
                },
                new object[]
                {
                new object[] { null },
                });

                c.Flip f = new c.Flip(dataRow);

                var exception = Assert.Throws<KException>(() => connection.Serialize(1, f));
                Assert.NotNull(exception.InnerException);

            }
        }
        [Test]
        public void ConnectionSerialiseThrowsIfGuidSerialisationIsNotSupported()
        {
            using (var connection = new c(2))
            {
                Assert.Throws<KException>(() => connection.Serialize(1, Guid.NewGuid()));
            }
        }

        [Test]
        public void ConnectionSerialiseThrowsIfDateTimeSerialisationIsNotSupported()
        {
            using (var connection = new c(0))
            {
                Assert.Throws<KException>(() => connection.Serialize(1, new DateTime(2020, 11, 11, 0, 0, 0, DateTimeKind.Utc)));
            }
        }

        [Test]
        public void ConnectionSerialiseThrowsIfTimeSpanSerialisationIsNotSupported()
        {
            using (var connection = new c(0))
            {
                Assert.Throws<KException>(() => connection.Serialize(1, new TimeSpan(470000)));
            }
        }

        [Test]
        public void ConnectionDeserialiseThrowsIfBufferIsNull()
        {
            using (var connection = new c(_testVersionNumber))
            {
                Assert.Throws<ArgumentNullException>(() => connection.Deserialize(null));
            }
        }

        [Test]
        public void ConnectionDeserialiseThrowsIfBufferIsException()
        {
            List<byte> buffer = new List<byte>();
            //message length etc
            buffer.AddRange(new byte[] { 1, 1, 0, 0, 20, 0, 0, 0 });
            //128 to indicate error
            buffer.Add(128);
            //error message
            buffer.AddRange(Encoding.ASCII.GetBytes("KDB+_Error"));
            //end of the error message
            buffer.Add(0);

            using (var connection = new c(_testVersionNumber))
            {
                Assert.Throws<KException>(() => connection.Deserialize(buffer.ToArray()));
            }
        }

        [Test]
        public void ConnectionDeserialiseThrowsExceptionWithExpectedMessage()
        {
            const string expected = "KDB+_Error";
            string errorMessage = null;

            List<byte> buffer = new List<byte>();
            //message length etc
            buffer.AddRange(new byte[] { 1, 1, 0, 0, 20, 0, 0, 0 });
            //128 to indicate error
            buffer.Add(128);
            //error message
            buffer.AddRange(Encoding.ASCII.GetBytes(expected));
            //end of the error message
            buffer.Add(0);

            using (var connection = new c(_testVersionNumber))
            {
                try
                {
                    connection.Deserialize(buffer.ToArray());
                }
                catch (KException kEx)
                {
                    errorMessage = kEx.Message;
                }

                Assert.AreEqual(expected, errorMessage);
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesBooleanTrueInput()
        {
            const bool expected = true;

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                object result = connection.Deserialize(serialisedData);

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesBooleanFalseInput()
        {
            const bool expected = false;

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                object result = connection.Deserialize(serialisedData);

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesGuidInput()
        {
            Guid expected = Guid.NewGuid();

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                object result = connection.Deserialize(serialisedData);

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesByteInput()
        {
            const byte expected = 47;

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                object result = connection.Deserialize(serialisedData);

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesShortInput()
        {
            const short expected = 47;

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                object result = connection.Deserialize(serialisedData);

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesIntInput()
        {
            const int expected = 47;

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                object result = connection.Deserialize(serialisedData);

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesLongInput()
        {
            const long expected = 47L;

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                object result = connection.Deserialize(serialisedData);

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesFloatInput()
        {
            const float expected = 47.14F;

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                object result = connection.Deserialize(serialisedData);

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesDoubleInput()
        {
            const double expected = 47.14;

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                object result = connection.Deserialize(serialisedData);

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesCharInput()
        {
            const char expected = 'k';

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                object result = connection.Deserialize(serialisedData);

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesStringInput()
        {
            const string expected = "Test_Input";

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                object result = connection.Deserialize(serialisedData);

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesDateTimeInput()
        {
            DateTime expected = new DateTime(2020, 11, 04, 0, 0, 0, DateTimeKind.Utc);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                object result = connection.Deserialize(serialisedData);

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesMonthInput()
        {
            c.Month expected = new c.Month(47);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                object result = connection.Deserialize(serialisedData);

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesDateInput()
        {
            c.Date expected = new c.Date(47);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                object result = connection.Deserialize(serialisedData);

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesKTimespanInput()
        {
            c.KTimespan expected = new c.KTimespan(4700);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                object result = connection.Deserialize(serialisedData);

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesMinuteInput()
        {
            c.Minute expected = new c.Minute(47);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                object result = connection.Deserialize(serialisedData);

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesSecondInput()
        {
            c.Second expected = new c.Second(47);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                object result = connection.Deserialize(serialisedData);

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesTimeSpanInput()
        {
            TimeSpan expected = new TimeSpan(470000);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                object result = connection.Deserialize(serialisedData);

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesDictInput()
        {
            c.Dict expected = new c.Dict(new string[] { "Key_1" }, new object[] { "Value_1" });

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                c.Dict result = connection.Deserialize(serialisedData) as c.Dict;

                Assert.IsNotNull(result);
                Assert.IsTrue(Enumerable.SequenceEqual(expected.x as string[], result.x as string[]));
                Assert.IsTrue(Enumerable.SequenceEqual(expected.y as object[], result.y as object[]));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesFlipInput()
        {
            c.Flip expected = new c.Flip(new c.Dict(new string[] { "Key_1" }, new object[] { "Value_1" }));

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                c.Flip result = connection.Deserialize(serialisedData) as c.Flip;

                Assert.IsNotNull(result);
                Assert.IsTrue(Enumerable.SequenceEqual(expected.x, result.x));
                Assert.IsTrue(Enumerable.SequenceEqual(expected.y, result.y));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesObjectArrayInput()
        {
            object[] expected = CreateTestArray(i => string.Format("Hello_{0}", i), 50);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                object[] result = connection.Deserialize(serialisedData) as object[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesBooleanArrayInput()
        {
            bool[] expected = CreateTestArray(i => i % 2 == 0, 50);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                bool[] result = connection.Deserialize(serialisedData) as bool[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesGuidArrayInput()
        {
            Guid[] expected = CreateTestArray(i => Guid.NewGuid(), 50);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                Guid[] result = connection.Deserialize(serialisedData) as Guid[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesByteArrayInput()
        {
            byte[] expected = CreateTestArray(i => (byte)i, 50);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                byte[] result = connection.Deserialize(serialisedData) as byte[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesShortArrayInput()
        {
            short[] expected = CreateTestArray(i => (short)i, 50);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                short[] result = connection.Deserialize(serialisedData) as short[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesIntArrayInput()
        {
            int[] expected = CreateTestArray(i => i, 50);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                int[] result = connection.Deserialize(serialisedData) as int[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesLongArrayInput()
        {
            long[] expected = CreateTestArray(i => (long)i, 50);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                long[] result = connection.Deserialize(serialisedData) as long[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }

        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesFloatArrayInput()
        {
            float[] expected = CreateTestArray(i => (float)i / 2, 50);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                float[] result = connection.Deserialize(serialisedData) as float[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesDoubleArrayInput()
        {
            double[] expected = CreateTestArray(i => (double)i / 2, 50);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                double[] result = connection.Deserialize(serialisedData) as double[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesCharArrayInput()
        {
            char[] expected = CreateTestArray(i => (char)i, 50);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                char[] result = connection.Deserialize(serialisedData) as char[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesStringArrayInput()
        {
            string[] expected = CreateTestArray(i => string.Format("Hello_{0}", i), 50);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                string[] result = connection.Deserialize(serialisedData) as string[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesDateTimeArrayInput()
        {
            DateTime[] expected = CreateTestArray(i => new DateTime(2020, 11, 11, 0, 0, i, DateTimeKind.Utc), 50);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                DateTime[] result = connection.Deserialize(serialisedData) as DateTime[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesMonthArrayInput()
        {
            c.Month[] expected = CreateTestArray(i => new c.Month(i), 50);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                c.Month[] result = connection.Deserialize(serialisedData) as c.Month[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesDateArrayInput()
        {
            c.Date[] expected = CreateTestArray(i => new c.Date(new DateTime(2020, 11, 11, 0, 0, i, DateTimeKind.Utc)), 50);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                c.Date[] result = connection.Deserialize(serialisedData) as c.Date[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesKTimeSpanArrayInput()
        {
            c.KTimespan[] expected = CreateTestArray(i => new c.KTimespan(i * 100), 50);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                c.KTimespan[] result = connection.Deserialize(serialisedData) as c.KTimespan[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesMinuteArrayInput()
        {
            c.Minute[] expected = CreateTestArray(i => new c.Minute(i), 50);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                c.Minute[] result = connection.Deserialize(serialisedData) as c.Minute[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesSecondArrayInput()
        {
            c.Second[] expected = CreateTestArray(i => new c.Second(i), 50);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                c.Second[] result = connection.Deserialize(serialisedData) as c.Second[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesTimeSpanArrayInput()
        {
            TimeSpan[] expected = CreateTestArray(i => new TimeSpan(i * 10000), 50);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                TimeSpan[] result = connection.Deserialize(serialisedData) as TimeSpan[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesObjectArrayInputWithZipEnabled()
        {
            object[] expected = CreateTestArray(i => string.Format("Hello_{0}", i), 2000);

            using (var connection = new c(_testVersionNumber))
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(1, expected);

                object[] result = connection.Deserialize(serialisedData) as object[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesBooleanArrayInputWithZipEnabled()
        {
            bool[] expected = CreateTestArray(i => i % 2 == 0, 2000);

            using (var connection = new c(_testVersionNumber))
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(0, expected);

                bool[] result = connection.Deserialize(serialisedData) as bool[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesGuidArrayInputWithZipEnabled()
        {
            Guid[] expected = CreateTestArray(i => Guid.NewGuid(), 2000);

            using (var connection = new c(_testVersionNumber))
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(0, expected);

                Guid[] result = connection.Deserialize(serialisedData) as Guid[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesByteArrayInputWithZipEnabled()
        {
            byte[] expected = CreateTestArray(i => (byte)i, 2000);

            using (var connection = new c(_testVersionNumber))
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(0, expected);

                byte[] result = connection.Deserialize(serialisedData) as byte[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesShortArrayInputWithZipEnabled()
        {
            short[] expected = CreateTestArray(i => (short)i, 2000);

            using (var connection = new c(_testVersionNumber))
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(0, expected);

                short[] result = connection.Deserialize(serialisedData) as short[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesIntArrayInputWithZipEnabled()
        {
            int[] expected = CreateTestArray(i => i, 2000);

            using (var connection = new c(_testVersionNumber))
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(0, expected);

                int[] result = connection.Deserialize(serialisedData) as int[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesLongArrayInputWithZipEnabled()
        {
            long[] expected = CreateTestArray(i => (long)i, 2000);

            using (var connection = new c(_testVersionNumber))
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(0, expected);

                long[] result = connection.Deserialize(serialisedData) as long[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }
        [Test]
        public void ConnectionSerialisesAndDeserialisesFloatArrayInputWithZipEnabled()
        {
            float[] expected = CreateTestArray(i => (float)i / 2, 50);

            using (var connection = new c(_testVersionNumber))
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(0, expected);

                float[] result = connection.Deserialize(serialisedData) as float[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesDoubleArrayInputWithZipEnabled()
        {
            double[] expected = CreateTestArray(i => (double)i / 2, 2000);

            using (var connection = new c(_testVersionNumber))
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(0, expected);

                double[] result = connection.Deserialize(serialisedData) as double[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesCharArrayInputWithZipEnabled()
        {
            // char serialisation only supports up to 128
            char[] expected = CreateTestArray(i =>
            {
                return (char)Math.Abs(new Random(i).Next(0, 127));
            }, 2000);

            using (var connection = new c(_testVersionNumber))
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(0, expected);

                char[] result = connection.Deserialize(serialisedData) as char[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }
        [Test]
        public void ConnectionSerialisesAndDeserialisesStringArrayInputWithZipEnabled()
        {
            string[] expected = CreateTestArray(i => string.Format("Hello_{0}", i), 2000);

            using (var connection = new c(_testVersionNumber))
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(0, expected);

                string[] result = connection.Deserialize(serialisedData) as string[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesDateTimeArrayInputWithZipEnabled()
        {
            DateTime[] expected = CreateTestArray(i => new DateTime(2020, 11, 11, 0, 0, i % 60, DateTimeKind.Utc), 2000);

            using (var connection = new c(_testVersionNumber))
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(0, expected);

                DateTime[] result = connection.Deserialize(serialisedData) as DateTime[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesMonthArrayInputWithZipEnabled()
        {
            c.Month[] expected = CreateTestArray(i => new c.Month(i), 2000);

            using (var connection = new c(_testVersionNumber))
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(0, expected);

                c.Month[] result = connection.Deserialize(serialisedData) as c.Month[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesDateArrayInputWithZipEnabled()
        {
            c.Date[] expected = CreateTestArray(i => new c.Date(new DateTime(2020, 11, 11, 0, 0, i % 60, DateTimeKind.Utc)), 2000);

            using (var connection = new c(_testVersionNumber))
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(0, expected);

                c.Date[] result = connection.Deserialize(serialisedData) as c.Date[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesKTimeSpanArrayInputWithZipEnabled()
        {
            c.KTimespan[] expected = CreateTestArray(i => new c.KTimespan(i * 100), 2000);

            using (var connection = new c(_testVersionNumber))
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(0, expected);

                c.KTimespan[] result = connection.Deserialize(serialisedData) as c.KTimespan[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesMinuteArrayInputWithZipEnabled()
        {
            c.Minute[] expected = CreateTestArray(i => new c.Minute(i), 2000);

            using (var connection = new c(_testVersionNumber))
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(0, expected);

                c.Minute[] result = connection.Deserialize(serialisedData) as c.Minute[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesSecondArrayInputWithZipEnabled()
        {
            c.Second[] expected = CreateTestArray(i => new c.Second(i), 2000);

            using (var connection = new c(_testVersionNumber))
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(0, expected);

                c.Second[] result = connection.Deserialize(serialisedData) as c.Second[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionSerialisesAndDeserialisesTimeSpanArrayInputWithZipEnabled()
        {
            TimeSpan[] expected = CreateTestArray(i => new TimeSpan(i * 10000), 2000);

            using (var connection = new c(_testVersionNumber))
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(0, expected);

                TimeSpan[] result = connection.Deserialize(serialisedData) as TimeSpan[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }


        private T[] CreateTestArray<T>(Func<int, T> elementBuilder, int arraySize)
        {
            T[] array = new T[arraySize];

            for (int i = 0; i < arraySize; i++)
            {
                array[i] = elementBuilder(i);
            }
            return array;
        }

                [Test]
        public void ConnectionDeserialisesBigEndianIntInput()
        {
            const int expected = 0x01020304;

            byte[] message = CreateBigEndianMessage(
                2,
                unchecked((byte)-6),
                0x01, 0x02, 0x03, 0x04);

            using (var connection = new c(_testVersionNumber))
            {
                object result = connection.Deserialize(message);

                Assert.AreEqual(expected, result);
                Assert.IsFalse(connection.IsSync);
                Assert.IsTrue(connection.IsResponse);
            }
        }

        [Test]
        public void ConnectionDeserialisesBigEndianLongInput()
        {
            const long expected = 0x0102030405060708L;

            byte[] message = CreateBigEndianMessage(
                1,
                unchecked((byte)-7),
                0x01, 0x02, 0x03, 0x04,
                0x05, 0x06, 0x07, 0x08);

            using (var connection = new c(_testVersionNumber))
            {
                object result = connection.Deserialize(message);

                Assert.AreEqual(expected, result);
                Assert.IsTrue(connection.IsSync);
                Assert.IsFalse(connection.IsResponse);
            }
        }

        [Test]
        public void ConnectionDeserialisesBigEndianFloatInput()
        {
            const float expected = 47.25F;

            byte[] value = BitConverter.GetBytes(expected);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(value);
            }

            byte[] payload = new byte[value.Length + 1];
            payload[0] = unchecked((byte)-8);
            Buffer.BlockCopy(value, 0, payload, 1, value.Length);

            byte[] message = CreateBigEndianMessage(1, payload);

            using (var connection = new c(_testVersionNumber))
            {
                object result = connection.Deserialize(message);

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void ConnectionDeserialisesBigEndianIntArrayInput()
        {
            int[] expected =
            {
                0x01020304,
                -2,
                int.MaxValue,
                int.MinValue
            };

            byte[] payload =
            {
                6,                  // q int list
                0,                  // attributes
                0, 0, 0, 4,        // number of elements

                0x01, 0x02, 0x03, 0x04,
                0xff, 0xff, 0xff, 0xfe,
                0x7f, 0xff, 0xff, 0xff,
                0x80, 0x00, 0x00, 0x00
            };

            byte[] message = CreateBigEndianMessage(1, payload);

            using (var connection = new c(_testVersionNumber))
            {
                int[] result = connection.Deserialize(message) as int[];

                Assert.IsNotNull(result);
                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionDeserialisesLegacyDatetimeInput()
        {
            DateTime expected =
                new DateTime(2000, 1, 2, 12, 0, 0, DateTimeKind.Unspecified);

            byte[] message = CreateLittleEndianScalar(
                unchecked((byte)-15),
                BitConverter.GetBytes(1.5));

            using (var connection = new c(_testVersionNumber))
            {
                object result = connection.Deserialize(message);

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void ConnectionDeserialisesLegacyDatetimePositiveInfinityAsMaxDateTime()
        {
            byte[] message = CreateLittleEndianScalar(
                unchecked((byte)-15),
                BitConverter.GetBytes(double.PositiveInfinity));

            using (var connection = new c(_testVersionNumber))
            {
                object result = connection.Deserialize(message);

                Assert.AreEqual(DateTime.MaxValue, result);
            }
        }

        [Test]
        public void ConnectionDeserialisesLegacyDatetimeNegativeInfinityAsMinimumKdbDateTime()
        {
            byte[] message = CreateLittleEndianScalar(
                unchecked((byte)-15),
                BitConverter.GetBytes(double.NegativeInfinity));

            DateTime expected = DateTime.MinValue.AddTicks(1);

            using (var connection = new c(_testVersionNumber))
            {
                object result = connection.Deserialize(message);

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void ConnectionDeserialisesGenericNull()
        {
            byte[] message = CreateLittleEndianMessage(
                1,
                101,
                0);

            using (var connection = new c(_testVersionNumber))
            {
                object result = connection.Deserialize(message);

                Assert.IsNull(result);
            }
        }

        [Test]
        public void ConnectionThrowsKExceptionForUnsupportedFunction()
        {
            byte[] message = CreateLittleEndianMessage(
                1,
                101,
                1);

            using (var connection = new c(_testVersionNumber))
            {
                KException exception =
                    Assert.Throws<KException>(() => connection.Deserialize(message));

                Assert.AreEqual("func", exception.Message);
            }
        }

        [Test]
        public void ConnectionSetsExpectedHeaderFlagsForSyncMessage()
        {
            using (var connection = new c(_testVersionNumber))
            {
                byte[] message = connection.Serialize(1, 42);

                connection.Deserialize(message);

                Assert.IsTrue(connection.IsSync);
                Assert.IsFalse(connection.IsResponse);
                Assert.IsFalse(connection.IsCompressed);
            }
        }

        [Test]
        public void ConnectionSetsExpectedHeaderFlagsForResponseMessage()
        {
            using (var connection = new c(_testVersionNumber))
            {
                byte[] message = connection.Serialize(2, 42);

                connection.Deserialize(message);

                Assert.IsFalse(connection.IsSync);
                Assert.IsTrue(connection.IsResponse);
                Assert.IsFalse(connection.IsCompressed);
            }
        }

        [Test]
        public void ConnectionFallsBackToUncompressedWhenDataDoesNotCompressEnough()
        {
            byte[] expected = new byte[10000];
            new Random(123456).NextBytes(expected);

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(0, expected, true);

                // Compression was requested, but effectively random data
                // cannot be reduced to the compressor's target size.
                Assert.AreEqual(0, serialisedData[2]);

                byte[] result = connection.Deserialize(serialisedData) as byte[];

                Assert.IsNotNull(result);
                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Test]
        public void ConnectionCompressesHighlyCompressibleInput()
        {
            byte[] expected = Enumerable.Repeat((byte)42, 10000).ToArray();

            using (var connection = new c(_testVersionNumber))
            {
                byte[] serialisedData = connection.Serialize(0, expected, true);

                Assert.AreEqual(1, serialisedData[2]);

                byte[] result = connection.Deserialize(serialisedData) as byte[];

                Assert.IsNotNull(result);
                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        private static byte[] CreateLittleEndianScalar(
            byte type,
            byte[] value)
        {
            byte[] payload = new byte[value.Length + 1];

            payload[0] = type;
            Buffer.BlockCopy(value, 0, payload, 1, value.Length);

            return CreateLittleEndianMessage(1, payload);
        }

        private static byte[] CreateLittleEndianMessage(
            byte messageType,
            params byte[] payload)
        {
            int length = 8 + payload.Length;
            byte[] message = new byte[length];

            message[0] = 1;
            message[1] = messageType;
            message[2] = 0;
            message[3] = 0;

            message[4] = (byte)length;
            message[5] = (byte)(length >> 8);
            message[6] = (byte)(length >> 16);
            message[7] = (byte)(length >> 24);

            Buffer.BlockCopy(payload, 0, message, 8, payload.Length);

            return message;
        }

        private static byte[] CreateBigEndianMessage(
            byte messageType,
            params byte[] payload)
        {
            int length = 8 + payload.Length;
            byte[] message = new byte[length];

            message[0] = 0;
            message[1] = messageType;
            message[2] = 0;
            message[3] = 0;

            message[4] = (byte)(length >> 24);
            message[5] = (byte)(length >> 16);
            message[6] = (byte)(length >> 8);
            message[7] = (byte)length;

            Buffer.BlockCopy(payload, 0, message, 8, payload.Length);

            return message;
        }
    }
}
