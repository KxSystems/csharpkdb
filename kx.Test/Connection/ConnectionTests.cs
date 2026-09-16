using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using kx.Test.TestUtils;
using NUnit.Framework;

namespace kx.Test.Connection
{
    [TestFixture]
    public class ConnectionTests
    {
        [Test]
        public void ConnectionInitialises()
        {
            using (var server = new TestableTcpServer())
            using (var c = new c("localhost", server.TestPort))
            {
                Assert.IsNotNull(c);
            }
        }

        [Test]
        public void ConnectionExposesSynchronousSocketTimeouts()
        {
            using (var server = new TestableTcpServer())
            using (var connection = new c("localhost", server.TestPort))
            {
                connection.SendTimeout = 1000;
                connection.ReceiveTimeout = 2000;

                Assert.AreEqual(1000, connection.SendTimeout);
                Assert.AreEqual(2000, connection.ReceiveTimeout);
            }
        }

        [Test]
        public void ConnectionThrowsIfHostIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new c(null as string, 8080));
        }

        [Test]
        public void ConnectionThrowsIfPortIsLessThanRange()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new c("localhost", System.Net.IPEndPoint.MinPort - 1));
        }

        [Test]
        public void ConnectionThrowsIfPortIsMoreThanRange()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new c("localhost", System.Net.IPEndPoint.MaxPort + 1));
        }

        [Test]
        public void ConnectionThrowsIfUserPassWordIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new c("localhost", 8080, null));
        }

        [Test]
        public void ConnectionThrowsIfAuthenticationDoesNotPass()
        {

            using (var server = new TestableFailingTcpServer())
            {
                Assert.Throws<KException>(() => new c("localhost", server.TestPort));
            }
        }

        [Test]
        public void ConnectionThrowsSerialisableExpectionIfAuthenticationDoesNotPass()
        {
            KException error = null;
            c connection = null;

            using (var server = new TestableFailingTcpServer())
            {
                try
                {
                    connection = new c("localhost", server.TestPort);
                }
                catch (KException ex)
                {
                    error = TestSerialisationHelper.SerialiseAndDeserialiseException(ex);
                }

                Assert.IsNull(connection);
                Assert.IsNotNull(error);
            }
        }

        [Test]
        public void ProtectedConstructorAndBufferStateAreAccessibleToDerivedTypes()
        {
            using (var connection = new TestConnection())
            {
                Assert.IsNull(connection.ExposedReadBuffer);
                Assert.IsFalse(connection.ExposedIsLittleEndian);
                Assert.AreEqual(0, connection.ExposedReadPosition);

                connection.ExposedReadPosition = 3;
                Assert.AreEqual(3, connection.ExposedReadPosition);
                Assert.Throws<ArgumentOutOfRangeException>(() => connection.ExposedReadPosition = -1);
            }
        }

        [Test]
        public void DisposeCanBeCalledMoreThanOnce()
        {
            var connection = new c(new MemoryStream());

            connection.Dispose();

            Assert.DoesNotThrow(connection.Dispose);
        }

        [Test]
        public void CloseClosesBothStreamAndSocket()
        {
            using (var server = new TestableTcpServer())
            {
                var connection = new c("localhost", server.TestPort);

                Assert.DoesNotThrow(connection.Close);
                connection.Dispose();
            }
        }

        [Test]
        public void ExplicitTlsOptionsConstructorAcceptsNullAsDisabled()
        {
            using (var server = new TestableTcpServer())
            using (var connection = new c("localhost", server.TestPort, Environment.UserName, 1024, null))
            {
                Assert.IsNotNull(connection);
            }
        }

        [Test]
        public async Task ParameterlessAsyncReadReturnsDeserialisedObject()
        {
            const int expected = 42;
            byte[] message;
            using (var serializer = new c(3))
            {
                message = serializer.Serialize(1, expected);
            }

            using (var stream = new MemoryStream(message))
            using (var connection = new c(stream))
            {
                Assert.AreEqual(expected, await connection.kAsync());
            }
        }

        [Test]
        public async Task ParameterlessAsyncHeaderReadPopulatesReadableBuffer()
        {
            const int expected = 42;
            byte[] message;
            using (var serializer = new c(3))
            {
                message = serializer.Serialize(1, expected);
            }

            using (var stream = new MemoryStream(message))
            using (var connection = new TestConnection(stream))
            {
                await connection.k0Async();

                Assert.AreEqual(expected, connection.ExposedReadObject());
            }
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task ParameterlessAsyncMessageWritesExpectedMessageType(int messageType)
        {
            const string expected = "payload";
            using (var stream = new MemoryStream())
            using (var connection = new c(stream))
            {
                if (messageType == 1)
                {
                    await connection.knAsync(expected);
                }
                else
                {
                    await connection.krAsync(expected);
                }

                byte[] message = stream.ToArray();
                Assert.AreEqual(messageType, message[1]);
                Assert.AreEqual(expected, connection.Deserialize(message));
            }
        }

        [Test]
        public async Task ProtectedParameterlessWriteAsyncWritesRequestedBytes()
        {
            byte[] expected = { 1, 2, 3, 4 };
            using (var stream = new MemoryStream())
            using (var connection = new TestConnection(stream))
            {
                await connection.ExposedWriteAsync(expected, expected.Length);

                Assert.IsTrue(expected.SequenceEqual(stream.ToArray()));
            }
        }

        private sealed class TestConnection : c
        {
            internal TestConnection()
            {
            }

            internal TestConnection(Stream stream)
                : base(stream)
            {
            }

            internal byte[] ExposedReadBuffer => ReadBuffer;

            internal int ExposedReadPosition
            {
                get => ReadPosition;
                set => ReadPosition = value;
            }

            internal bool ExposedIsLittleEndian => IsLittleEndian;

            internal object ExposedReadObject()
            {
                return ReadObject();
            }

            internal Task ExposedWriteAsync(byte[] bytes, int number)
            {
                return WriteAsync(bytes, number);
            }
        }
    }
}
