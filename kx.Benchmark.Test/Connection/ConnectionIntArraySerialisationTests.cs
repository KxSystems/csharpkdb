using System.Linq;
using BenchmarkDotNet.Attributes;
using NUnit.Framework;

namespace kx.Benchmark.Test.Connection
{
    [MemoryDiagnoser]
    [MedianColumn]
    [MaxColumn]
    public class ConnectionIntArraySerialisationTests
    {
        private int[] _data;

        [Params(1000, 10000, 100000, 500000)]
        public int Number { get; set; }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesIntArrayInput()
        {
            int[] expected = _data;

            using (var connection = new c())
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                int[] result = connection.Deserialize(serialisedData) as int[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesIntArrayInputWithZipEnabled()
        {
            int[] expected = _data;

            using (var connection = new c())
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(1, expected);

                int[] result = connection.Deserialize(serialisedData) as int[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [GlobalSetup]
        public void Setup()
        {
            _data = new int[Number];
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = i;
            }
        }
    }
}
