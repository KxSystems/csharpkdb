using System.Linq;
using BenchmarkDotNet.Attributes;
using NUnit.Framework;

namespace kx.Benchmark.Test.Connection
{
    [MemoryDiagnoser]
    [MedianColumn]
    [MaxColumn]
    public class ConnectionMinuteArraySerialisationTests
    {
        private c.Minute[] _data;

        [Params(1000, 10000, 100000, 500000)]
        public int Number { get; set; }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesMinuteArrayInput()
        {
            c.Minute[] expected = _data;

            using (var connection = new c())
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                c.Minute[] result = connection.Deserialize(serialisedData) as c.Minute[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [GlobalSetup]
        public void Setup()
        {
            _data = new c.Minute[Number];
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = new c.Minute(i);
            }
        }
    }
}
