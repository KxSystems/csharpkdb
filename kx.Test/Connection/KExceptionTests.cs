using System;
using NUnit.Framework;

namespace kx.Test.Connection
{
    [TestFixture]
    public class KExceptionTests
    {
        [Test]
        public void DefaultConstructorCreatesExceptionWithDefaultMessage()
        {
            var exception = new KException();

            Assert.IsNull(exception.InnerException);
            Assert.IsNotEmpty(exception.Message);
        }

        [Test]
        public void MessageConstructorPreservesMessage()
        {
            const string expected = "kdb error";

            var exception = new KException(expected);

            Assert.AreEqual(expected, exception.Message);
        }

        [Test]
        public void InnerExceptionConstructorPreservesMessageAndCause()
        {
            const string expected = "serialization failed";
            var cause = new InvalidOperationException("cause");

            var exception = new KException(expected, cause);

            Assert.AreEqual(expected, exception.Message);
            Assert.AreSame(cause, exception.InnerException);
        }
    }
}
