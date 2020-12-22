using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using NUnit.Framework;

namespace kx.Benchmark.Test.Connection
{
    [MemoryDiagnoser]
    [MedianColumn]
    [MaxColumn]
    public class ConnectionShortArraySerialisationTests
    {
        private short[] _data;

        [Params(1000, 10000, 100000, 500000)]
        public int Number { get; set; }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesByteArrayInput()
        {
            short[] expected = _data;

            using (var connection = new c(3))
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                short[] result = connection.Deserialize(serialisedData) as short[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesByteArrayInputWithZipEnabled()
        {
            short[] expected = _data;

            using (var connection = new c(3))
            {
                connection.IsZipEnabled = true;

                byte[] serialisedData = connection.Serialize(1, expected);

                short[] result = connection.Deserialize(serialisedData) as short[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [GlobalSetup]
        public void Setup()
        {
            Random random = new Random();

            _data = new short[Number];
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = (short)random.Next(short.MinValue, short.MaxValue);
            }
        }
    }
}
