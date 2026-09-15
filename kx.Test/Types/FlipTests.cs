using System;
using NUnit.Framework;

namespace kx.Test.Types
{
    [TestFixture]
    public class FlipTests
    {
        private static readonly string[] FlipKeys = {"Key_1"};
        private static readonly object[] FlipValues = {new object[] { "Value_1" }};
        [Test]
        public void FlipInitialises()
        {
            var flip = new c.Flip(new c.Dict(FlipKeys, FlipValues));

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
            var flip = new c.Flip(new c.Dict(FlipKeys, FlipValues));

            object result = flip.at("Key_1");

            Assert.IsNotNull(result);
            Assert.AreEqual("Value_1", result);
        }

        [Test]
        public void FlipAtThrowsIfColumnNameIfNotFound()
        {
            var flip = new c.Flip(new c.Dict(FlipKeys, FlipValues));

            Assert.Throws<IndexOutOfRangeException>(() => flip.at("Aardvark"));
        }

        [Test]
        public void FlipAtThrowsIfColumnNameIfNull()
        {
            var flip = new c.Flip(new c.Dict(FlipKeys, FlipValues));

            Assert.Throws<IndexOutOfRangeException>(() => flip.at(null));
        }
    }
}
