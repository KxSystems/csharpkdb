using System;
using kx.Test.TestUtils;
using NUnit.Framework;

namespace kx.Test.Connection
{
    [TestFixture]
    public class ConnectionArrayTests
    {
        [Test]
        public void ConnectionReturnsExpectedLengthForDict()
        {
            const int expected = 1;

            c.Dict dict = new c.Dict(new[] { "Key_1" }, new object[] { "Value_1" });

            int count = c.n(dict);

            Assert.AreEqual(expected, count);
        }

        [Test]
        public void ConnectionReturnsExpectedLengthForFlip()
        {
            const int expected = 1;

            c.Flip flip = new c.Flip(new c.Dict(new[] { "Key_1" }, new object[] { new object[] { "Value_1" } }));

            int count = c.n(flip);

            Assert.AreEqual(expected, count);
        }

        [Test]
        public void ConnectionReturnsExpectedLengthForCharArray()
        {
            const int expected = 8;

            char[] array = "Aardvark".ToCharArray();

            int count = c.n(array);

            Assert.AreEqual(expected, count);
        }

        [Test]
        public void ConnectionReturnsExpectedLengthForObjectArray()
        {
            const int expected = 1;

            object[] array = new[] { "Aardvark" };

            int count = c.n(array);

            Assert.AreEqual(expected, count);
        }

        [Test]
        public void ConnectionThrowsIfLengthForNull()
        {
            Assert.Throws<ArgumentNullException>(() => c.n(null));
        }

        [Test]
        public void ConnectionAtThrowsIfXIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => c.at(null, 0));
        }

        [Test]
        public void ConnectionAtReturnsExpectedObjectFromArrayIndex()
        {
            const string expected = "foo";

            object result = c.at(new object[] { "foo", "bar" }, 0);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionAtReturnsNullIfObjectFromArrayIndexIsNull()
        {
            object result = c.at(new object[] { null, "bar" }, 0);

            Assert.IsNull(result);
        }

        [Test]
        public void ConnectionAtReturnsNullIfObjectFromArrayIndexIsQNull()
        {
            object result = c.at(new object[] { string.Empty, "bar" }, 0);

            Assert.IsNull(result);
        }

        [Test]
        public void ConnectionTdThrowsIfXIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => c.td(null));
        }

        [Test]
        public void ConnectionTableReturnsExpectedTableIfObjectIsFlip()
        {
            c.Flip flip = new c.Flip(new c.Dict(new[] { "Key_1" }, new object[] { new object[] { "Value_1" } }));

            object result = c.td(flip);

            Assert.IsNotNull(result);
            Assert.IsTrue(result is c.Flip);
        }

        [Test]
        public void ConnectionTableReturnsExpectedTableIfObjectIsDict()
        {
            c.Dict dict = new c.Dict
                (
                    new c.Flip(new c.Dict(new[] { "Key_1" }, new object[] { new object[] { "Value_1" } })),
                    new c.Flip(new c.Dict(new[] { "Key_1" }, new object[] { new object[] { "Value_1" } }))
                );

            object result = c.td(dict);

            Assert.IsNotNull(result);
            Assert.IsTrue(result is c.Flip);
        }
    }
}
