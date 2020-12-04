using System.Linq;
using BenchmarkDotNet.Attributes;
using NUnit.Framework;

namespace kx.Benchmark.Test.Connection
{
    [MemoryDiagnoser]
    [MedianColumn]
    [MaxColumn]
    public class ConnectionDoubleArraySerialisationTests
    {
        private double[] _data;

        [Params(1000, 10000, 100000, 500000)]
        public int Number { get; set; }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesDoubleArrayInput()
        {
            double[] expected = _data;

            using (var connection = new c())
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                double[] result = connection.Deserialize(serialisedData) as double[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesDoubleArrayInputWithZipEnabled()
        {
            double[] expected = _data;

            using (var connection = new c())
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(1, expected);

                double[] result = connection.Deserialize(serialisedData) as double[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [GlobalSetup]
        public void Setup()
        {
            _data = new double[Number];
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = (double)i / 2;
            }
        }
    }
}
