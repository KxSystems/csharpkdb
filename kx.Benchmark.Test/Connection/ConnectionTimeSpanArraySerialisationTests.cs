using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using NUnit.Framework;

namespace kx.Benchmark.Test.Connection
{
    [MemoryDiagnoser]
    [MedianColumn]
    [MaxColumn]
    public class ConnectionTimeSpanArraySerialisationTests
    {
        private TimeSpan[] _data;

        [Params(1000, 10000, 100000, 500000)]
        public int Number { get; set; }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesTimeSpanArrayInput()
        {
            TimeSpan[] expected = _data;

            using (var connection = new c())
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                TimeSpan[] result = connection.Deserialize(serialisedData) as TimeSpan[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [GlobalSetup]
        public void Setup()
        {
            _data = new TimeSpan[Number];

            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = new TimeSpan(new Random(i).Next(1, 2000) * 10000);
            }
        }
    }
}
