using System.Linq;
using BenchmarkDotNet.Attributes;
using NUnit.Framework;

namespace kx.Benchmark.Test.Connection
{
    [MemoryDiagnoser]
    [MedianColumn]
    [MaxColumn]
    public class ConnectionObjectArraySerialisationTests
    {
        private object[] _data;

        [Params(1000, 10000, 100000, 500000)]
        public int Number { get; set; }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesObjectArrayInput()
        {
            object[] expected = _data;

            using (var connection = new c())
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                object[] result = connection.Deserialize(serialisedData) as object[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesObjectArrayInputWithZipEnabled()
        {
            object[] expected = _data;

            using (var connection = new c())
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(1, expected);

                object[] result = connection.Deserialize(serialisedData) as object[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [GlobalSetup]
        public void Setup()
        {
            _data = new object[Number];
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = string.Format("Hello_{0}", i);
            }
        }
    }
}
