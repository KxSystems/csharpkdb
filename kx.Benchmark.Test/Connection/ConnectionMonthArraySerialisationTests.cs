using System.Linq;
using BenchmarkDotNet.Attributes;
using NUnit.Framework;

namespace kx.Benchmark.Test.Connection
{
    [MemoryDiagnoser]
    [MedianColumn]
    [MaxColumn]
    public class ConnectionMonthArraySerialisationTests
    {
        private c.Month[] _data;

        [Params(1000, 10000, 100000, 500000)]
        public int Number { get; set; }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesMonthArrayInput()
        {
            c.Month[] expected = _data;

            using (var connection = new c())
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                c.Month[] result = connection.Deserialize(serialisedData) as c.Month[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [GlobalSetup]
        public void Setup()
        {
            _data = new c.Month[Number];
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = new c.Month(i);
            }
        }
    }
}
