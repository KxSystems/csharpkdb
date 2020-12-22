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
    }
}
