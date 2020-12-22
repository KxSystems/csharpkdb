using System.Linq;
using BenchmarkDotNet.Attributes;
using NUnit.Framework;

namespace kx.Benchmark.Test.Connection
{
    [MemoryDiagnoser]
    [MedianColumn]
    [MaxColumn]
    public class ConnectionStringArraySerialisationTests
    {
        private string[] _data;

        [Params(1000, 10000, 100000, 500000)]
        public int Number { get; set; }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesStringArrayInput()
        {
            string[] expected = _data;

            using (var connection = new c(3))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                string[] result = connection.Deserialize(serialisedData) as string[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesStringArrayInputWithZipEnabled()
        {
            string[] expected = _data;

            using (var connection = new c(3))
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(1, expected);

                string[] result = connection.Deserialize(serialisedData) as string[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [GlobalSetup]
        public void Setup()
        {
            _data = new string[Number];
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = string.Format("Hello_{0}", i);
            }
        }
    }
}
