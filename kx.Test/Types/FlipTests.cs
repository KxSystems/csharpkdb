using System;
using NUnit.Framework;

namespace kx.Test.Types
{
    [TestFixture]
    public class FlipTests
    {
        [Test]
        public void FlipInitialises()
        {
            var flip = new c.Flip(new c.Dict(new string[] { "Key_1" }, new object[] { "Value_1" }));

            Assert.IsNotNull(flip);
        }

        [Test]
        public void FlipThrowsIfDictIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new c.Flip(null as c.Dict));
        }

        [Test]
        public void FlipAtReturnsValueForColumnName()
        {
            var flip = new c.Flip(new c.Dict(new string[] { "Key_1" }, new object[] { "Value_1" }));

            object result = flip.at("Key_1");

            Assert.IsNotNull(result);
            Assert.AreEqual("Value_1", result);
        }

        [Test]
        public void FlipAtThrowsIfColumnNameIfNotFound()
        {
            var flip = new c.Flip(new c.Dict(new string[] { "Key_1" }, new object[] { "Value_1" }));

            Assert.Throws<IndexOutOfRangeException>(() => flip.at("Aardvark"));
        }

        [Test]
        public void FlipAtThrowsIfColumnNameIfNull()
        {
            var flip = new c.Flip(new c.Dict(new string[] { "Key_1" }, new object[] { "Value_1" }));

            Assert.Throws<IndexOutOfRangeException>(() => flip.at(null));
        }
    }
}
