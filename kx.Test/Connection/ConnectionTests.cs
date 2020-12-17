using System;
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
    }
}
