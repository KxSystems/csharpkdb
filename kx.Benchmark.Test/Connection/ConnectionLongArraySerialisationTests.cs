using System.Linq;
using BenchmarkDotNet.Attributes;
using NUnit.Framework;

namespace kx.Benchmark.Test.Connection
{
    [MemoryDiagnoser]
    [MedianColumn]
    [MaxColumn]
    public class ConnectionLongArraySerialisationTests
    {
        private long[] _data;

        [Params(1000, 10000, 100000, 500000)]
        public int Number { get; set; }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesLongArrayInput()
        {
            long[] expected = _data;

            using (var connection = new c(3))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                long[] result = connection.Deserialize(serialisedData) as long[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesLongArrayInputWithZipEnabled()
        {
            long[] expected = _data;

            using (var connection = new c(3))
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(1, expected);

                long[] result = connection.Deserialize(serialisedData) as long[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [GlobalSetup]
        public void Setup()
        {
            _data = new long[Number];
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = i;
            }
        }
    }
}
