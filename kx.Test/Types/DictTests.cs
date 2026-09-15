using System;
using NUnit.Framework;

namespace kx.Test.Types
{
    [TestFixture]
    public class DictTests
    {
        private static readonly string[] DictKeys = {"Key_1"};
        private static readonly object[] DictValues = {new object[] { "Value_1" }};
        [Test]
        public void DictInitialises()
        {
            var dict = new c.Dict(DictKeys, DictValues);

            Assert.IsNotNull(dict);
        }

        [Test]
        public void DictThrowsIfKeysIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new c.Dict(null, DictValues));
        }

        [Test]
        public void DictThrowsIfValuesIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new c.Dict(DictKeys, null));
        }
    }
}
