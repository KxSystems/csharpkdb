using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using NUnit.Framework;

namespace kx.Benchmark.Test.Connection
{
    [MemoryDiagnoser]
    [MedianColumn]
    [MaxColumn]
    public class ConnectionDateTimeArraySerialisationTests
    {
        private DateTime[] _data;

        [Params(1000, 10000, 100000, 500000)]
        public int Number { get; set; }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesDateTimeArrayInput()
        {
            DateTime[] expected = _data;

            using (var connection = new c())
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                DateTime[] result = connection.Deserialize(serialisedData) as DateTime[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [GlobalSetup]
        public void Setup()
        {
            DateTime dt = new DateTime(2020, 11, 11, 0, 0, 0, DateTimeKind.Utc);
            _data = new DateTime[Number];
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = dt.AddSeconds(i);
            }
        }
    }
}
