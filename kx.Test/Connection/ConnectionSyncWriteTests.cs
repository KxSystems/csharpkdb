using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using Moq;
using NUnit.Framework;

namespace kx.Test.Connection
{
    [TestFixture]
    public class ConnectionSyncWriteTests
    {
        [Test]
        public void ConnectionWritesThrowsIfSingleParamSIsNull()
        {
            List<byte> bytesWritten = new List<byte>();

            Mock<Stream> testStream = CreateTestStream(bytesWritten);

            using (var connection = new c(testStream.Object))
            {
                Assert.Throws<ArgumentNullException>(() => connection.k(null));
            }
        }

        [Test]
        public void ConnectionWritesThrowsIf2ParamSIsNull()
        {
            object expected = "param";

            List<byte> bytesWritten = new List<byte>();

            Mock<Stream> testStream = CreateTestStream(bytesWritten);

            using (var connection = new c(testStream.Object))
            {
                Assert.Throws<ArgumentNullException>(() => connection.k(null,expected));
            }
        }

        [Test]
        public void ConnectionWritesThrowsIf3ParamSIsNull()
        {
            object expected = "param";

            List<byte> bytesWritten = new List<byte>();

            Mock<Stream> testStream = CreateTestStream(bytesWritten);

            using (var connection = new c(testStream.Object))
            {
                Assert.Throws<ArgumentNullException>(() => connection.k(null,expected,expected));
            }
        }

        [Test]
        public void ConnectionWritesThrowsIf4ParamSIsNull()
        {
            object expected = "param";

            List<byte> bytesWritten = new List<byte>();

            Mock<Stream> testStream = CreateTestStream(bytesWritten);

            using (var connection = new c(testStream.Object))
            {
                Assert.Throws<ArgumentNullException>(() => connection.k(null,expected,expected,expected));
            }
        }

        [Test]
        public void ConnectionWritesExpectedObjectParameterToClientStreamSynchronously()
        {
            object expected = "param1";

            List<byte> bytesWritten = new List<byte>();

            Mock<Stream> testStream = CreateTestStream(bytesWritten);

            using (var connection = new c(testStream.Object))
            {
                connection.k(expected);

                object result = connection.Deserialize(bytesWritten.ToArray());

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void ConnectionWritesExpectedStringExpressionToClientStreamSynchronously()
        {
            const string expression = "test_expression";

            List<byte> bytesWritten = new List<byte>();

            Mock<Stream> testStream = CreateTestStream(bytesWritten);

            using (var connection = new c(testStream.Object))
            {
                connection.k(expression);

                object result = connection.Deserialize(bytesWritten.ToArray());

                Assert.AreEqual(expression, result);
            }
        }

        [Test]
        public void ConnectionWritesExpectedStringExpressionAndObjectParameterToClientStreamSynchronously()
        {
            const string expression = "test_expression";
            object parameter1 = 1;

            List<byte> bytesWritten = new List<byte>();

            Mock<Stream> testStream = CreateTestStream(bytesWritten);

            using (var connection = new c(testStream.Object))
            {
                connection.k(expression, parameter1);

                object[] result = connection.Deserialize(bytesWritten.ToArray()) as object[];

                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.Length);
                Assert.AreEqual(expression, new string(result[0] as char[]));
                Assert.AreEqual(parameter1, result[1]);
            }
        }

        [Test]
        public void ConnectionWritesExpectedStringExpressionAndTwoObjectParametersToClientStreamSynchronously()
        {
            const string expression = "test_expression";
            object parameter1 = 1;
            object parameter2 = 2;

            List<byte> bytesWritten = new List<byte>();

            Mock<Stream> testStream = CreateTestStream(bytesWritten);

            using (var connection = new c(testStream.Object))
            {
                connection.k(expression, parameter1, parameter2);

                object[] result = connection.Deserialize(bytesWritten.ToArray()) as object[];

                Assert.IsNotNull(result);
                Assert.AreEqual(3, result.Length);
                Assert.AreEqual(expression, new string(result[0] as char[]));
                Assert.AreEqual(parameter1, result[1]);
                Assert.AreEqual(parameter2, result[2]);
            }
        }

        [Test]
        public void ConnectionWritesExpectedStringExpressionAndThreeObjectParametersToClientStreamSynchronously()
        {
            const string expression = "test_expression";
            object parameter1 = 1;
            object parameter2 = 2;
            object parameter3 = 3;

            List<byte> bytesWritten = new List<byte>();

            Mock<Stream> testStream = CreateTestStream(bytesWritten);

            using (var connection = new c(testStream.Object))
            {
                connection.k(expression, parameter1, parameter2, parameter3);

                object[] result = connection.Deserialize(bytesWritten.ToArray()) as object[];

                Assert.IsNotNull(result);
                Assert.AreEqual(4, result.Length);
                Assert.AreEqual(expression, new string(result[0] as char[]));
                Assert.AreEqual(parameter1, result[1]);
                Assert.AreEqual(parameter2, result[2]);
                Assert.AreEqual(parameter3, result[3]);
            }
        }

        private Mock<Stream> CreateTestStream(List<byte> bytesWritten)
        {
            Mock<Stream> testStream = new Mock<Stream>();

            //simulate sync response read
            testStream.Setup(s => s.Read(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns<byte[], int, int>((a, b, c) =>
                {
                    //simulate header read
                    if (a.Length == 8)
                    {
                        a[0] = 1;
                        a[1] = 2;
                        a[2] = 0;
                        a[3] = 0;
                        a[4] = 10;
                    }
                    //simulate empty string response
                    if (a.Length == 2)
                    {
                        a[0] = 245;
                    }
                    return a.Length;
                });

            //record bytes written to test-stream.
            testStream.Setup(s => s.Write(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Callback<byte[], int, int>((a, b, c) =>
                {
                    var content = a
                        .Skip(b)
                        .Take(c)
                        .ToArray();

                    bytesWritten.AddRange(content);
                });

            return testStream;
        }
    }
}