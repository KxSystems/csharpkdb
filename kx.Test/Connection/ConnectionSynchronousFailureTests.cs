using System.IO;
using Moq;
using NUnit.Framework;

namespace kx.Test.Connection
{
    [TestFixture]
    public class ConnectionSynchronousFailureTests
    {
        [Test]
        public void SynchronousWriteFailureClosesConnection()
        {
            var stream = new Mock<Stream>();
            stream.Setup(s => s.Write(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new IOException("write timeout"));

            using (var connection = new c(stream.Object))
            {
                Assert.Throws<IOException>(() => connection.ks("test"));
                stream.Verify(s => s.Close(), Times.Once);
            }
        }

        [Test]
        public void SynchronousReadFailureClosesConnection()
        {
            var stream = new Mock<Stream>();
            stream.Setup(s => s.Read(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new IOException("read timeout"));

            using (var connection = new c(stream.Object))
            {
                Assert.Throws<IOException>(() => connection.k0());
                stream.Verify(s => s.Close(), Times.Once);
            }
        }
    }
}
