using System;
using NUnit.Framework;

namespace kx.Test.Types
{
    [TestFixture]
    public class DictTests
    {
        [Test]
        public void DictInitialises()
        {
            var dict = new c.Dict(new string[] { "Key_1" }, new object[] { "Value_1" });

            Assert.IsNotNull(dict);
        }

        [Test]
        public void DictThrowsIfKeysIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new c.Dict(null, new object[] { "Value_1" }));
        }

        [Test]
        public void DictThrowsIfValuesIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new c.Dict(new string[] { "Key_1" }, null));
        }
    }
}
