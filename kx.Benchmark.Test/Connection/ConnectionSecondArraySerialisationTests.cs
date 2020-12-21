using System.Linq;
using BenchmarkDotNet.Attributes;
using NUnit.Framework;

namespace kx.Benchmark.Test.Connection
{
    [MemoryDiagnoser]
    [MedianColumn]
    [MaxColumn]
    public class ConnectionSecondArraySerialisationTests
    {
        private c.Second[] _data;

        [Params(1000, 10000, 100000, 500000)]
        public int Number { get; set; }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesSecondArrayInput()
        {
            c.Second[] expected = _data;

            using (var connection = new c())
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                c.Second[] result = connection.Deserialize(serialisedData) as c.Second[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesSecondArrayInputWithZipEnabled()
        {
            c.Second[] expected = _data;

            using (var connection = new c())
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(1, expected);

                c.Second[] result = connection.Deserialize(serialisedData) as c.Second[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [GlobalSetup]
        public void Setup()
        {
            _data = new c.Second[Number];
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = new c.Second(i);
            }
        }
    }
}
