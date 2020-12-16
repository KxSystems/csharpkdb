using System.Linq;
using BenchmarkDotNet.Attributes;
using NUnit.Framework;

namespace kx.Benchmark.Test.Connection
{
    [MemoryDiagnoser]
    [MedianColumn]
    [MaxColumn]
    public class ConnectionBooleanArraySerialisationTests
    {
        private bool[] _data;

        [Params(1000, 10000, 100000, 500000)]
        public int Number { get; set; }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesBooleanArrayInput()
        {
            bool[] expected = _data;

            using (var connection = new c())
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                bool[] result = connection.Deserialize(serialisedData) as bool[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesBooleanArrayInputWithZipEnabled()
        {
            bool[] expected = _data;

            using (var connection = new c())
            {
                connection.IsZipEnabled = true;
                
                byte[] serialisedData = connection.Serialize(1, expected);

                bool[] result = connection.Deserialize(serialisedData) as bool[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [GlobalSetup]
        public void Setup()
        {
            _data = new bool[Number];
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = i % 2 == 0;
            }
        }
    }
}
