using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using NUnit.Framework;

namespace kx.Benchmark.Test.Connection
{
    [MemoryDiagnoser]
    [MedianColumn]
    [MaxColumn]
    public class ConnectionDateArraySerialisationTests
    {
        private c.Date[] _data;

        [Params(1000, 10000, 100000, 500000)]
        public int Number { get; set; }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesDateArrayInput()
        {
            c.Date[] expected = _data;

            using (var connection = new c(3))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                c.Date[] result = connection.Deserialize(serialisedData) as c.Date[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesDateArrayInputWithZipEnabled()
        {
            c.Date[] expected = _data;

            using (var connection = new c(3))
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(1, expected);

                c.Date[] result = connection.Deserialize(serialisedData) as c.Date[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [GlobalSetup]
        public void Setup()
        {
            DateTime dt = new DateTime(2020, 11, 11, 0, 0, 0, DateTimeKind.Utc);
            _data = new c.Date[Number];
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = new c.Date(dt.AddSeconds(i));
            }
        }
    }
}
