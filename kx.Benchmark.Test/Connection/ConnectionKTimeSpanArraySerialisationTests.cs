using System.Linq;
using BenchmarkDotNet.Attributes;
using NUnit.Framework;

namespace kx.Benchmark.Test.Connection
{
    [MemoryDiagnoser]
    [MedianColumn]
    [MaxColumn]
    public class ConnectionKTimeSpanArraySerialisationTests
    {
        private c.KTimespan[] _data;

        [Params(1000, 10000, 100000, 500000)]
        public int Number { get; set; }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesKTimeSpanArrayInput()
        {
            c.KTimespan[] expected = _data;

            using (var connection = new c())
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                c.KTimespan[] result = connection.Deserialize(serialisedData) as c.KTimespan[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [GlobalSetup]
        public void Setup()
        {
            _data = new c.KTimespan[Number];
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = new c.KTimespan(i);
            }
        }
    }
}
