using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace kx.Test.Connection
{
    [TestFixture]
    public class ConnectionAsyncWriteTests
    {
        [Test]
        public void ConnectionWritesExpectedObjectParameterToClientStreamAsynchronously()
        {
            object expected = "param1";

            List<byte> bytesWritten = new List<byte>();

            Mock<Stream> testStream = CreateTestStream(bytesWritten);

            using (var connection = new c(testStream.Object))
            {
                connection.ks(expected);

                object result = connection.Deserialize(bytesWritten.ToArray());

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public async Task ConnectionWritesExpectedObjectParameterToClientStreamAsync()
        {
            object expected = "param1";

            using (MemoryStream s = new MemoryStream())
            {
                using (var connection = new c(s))
                {
                    await connection.ksAsync(expected);

                    object result = connection.Deserialize(s.GetBuffer());

                    Assert.AreEqual(expected, result);
                }
            }
        }

        [Test]
        public void ConnectionWritesExpectedStringExpressionToClientStreamAsynchronously()
        {
            const string expected = "test_expression";

            List<byte> bytesWritten = new List<byte>();

            Mock<Stream> testStream = CreateTestStream(bytesWritten);

            using (var connection = new c(testStream.Object))
            {
                connection.ks(expected);

                object result = connection.Deserialize(bytesWritten.ToArray());

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public async Task ConnectionWritesExpectedStringExpressionToClientStreamAsync()
        {
            const string expected = "test_expression";

            using (MemoryStream s = new MemoryStream())
            {
                using (var connection = new c(s))
                {
                    await connection.ksAsync(expected);

                    object result = connection.Deserialize(s.GetBuffer());

                    Assert.AreEqual(expected, result);
                }
            }
        }

        [Test]
        public void ConnectionWritesExpectedStringExpressionAndParameterToClientStreamAsynchronously()
        {
            const string expression = "test_expression";
            object parameter1 = 1;

            List<byte> bytesWritten = new List<byte>();

            Mock<Stream> testStream = CreateTestStream(bytesWritten);

            using (var connection = new c(testStream.Object))
            {
                connection.ks(expression, parameter1);

                object[] result = connection.Deserialize(bytesWritten.ToArray()) as object[];

                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.Length);
                Assert.AreEqual(expression, new string(result[0] as char[]));
                Assert.AreEqual(parameter1, result[1]);
            }
        }

        [Test]
        public async Task ConnectionWritesExpectedStringExpressionAndParameterToClientStreamAsync()
        {
            const string expression = "test_expression";
            object parameter1 = 1;

            using (MemoryStream s = new MemoryStream())
            {
                using (var connection = new c(s))
                {
                    await connection.ksAsync(expression, parameter1);

                    object[] result = connection.Deserialize(s.GetBuffer()) as object[];

                    Assert.IsNotNull(result);
                    Assert.AreEqual(2, result.Length);
                    Assert.AreEqual(expression, new string(result[0] as char[]));
                    Assert.AreEqual(parameter1, result[1]);
                }
            }
        }

        [Test]
        public void ConnectionWritesExpectedStringExpressionAndParametersToClientStreamAsynchronously()
        {
            const string expression = "test_expression";
            object parameter1 = 1;
            object parameter2 = 2;

            List<byte> bytesWritten = new List<byte>();

            Mock<Stream> testStream = CreateTestStream(bytesWritten);

            using (var connection = new c(testStream.Object))
            {
                connection.ks(expression, parameter1, parameter2);

                object[] result = connection.Deserialize(bytesWritten.ToArray()) as object[];

                Assert.IsNotNull(result);
                Assert.AreEqual(3, result.Length);
                Assert.AreEqual(expression, new string(result[0] as char[]));
                Assert.AreEqual(parameter1, result[1]);
                Assert.AreEqual(parameter2, result[2]);
            }
        }

        [Test]
        public async Task ConnectionWritesExpectedStringExpressionAndParametersToClientStreamAsync()
        {
            const string expression = "test_expression";
            object parameter1 = 1;
            object parameter2 = 2;

            using (MemoryStream s = new MemoryStream())
            {
                using (var connection = new c(s))
                {
                    await connection.ksAsync(expression, parameter1, parameter2);

                    object[] result = connection.Deserialize(s.GetBuffer()) as object[];

                    Assert.IsNotNull(result);
                    Assert.AreEqual(3, result.Length);
                    Assert.AreEqual(expression, new string(result[0] as char[]));
                    Assert.AreEqual(parameter1, result[1]);
                    Assert.AreEqual(parameter2, result[2]);
                }
            }
        }

        private Mock<Stream> CreateTestStream(List<byte> bytesWritten)
        {
            Mock<Stream> testStream = new Mock<Stream>();

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