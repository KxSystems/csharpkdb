using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using NUnit.Framework;

namespace kx.Benchmark.Test.Connection
{
    [MemoryDiagnoser]
    [MedianColumn]
    [MaxColumn]
    public class ConnectionCharArraySerialisationTests
    {
        private char[] _data;

        [Params(1000, 10000, 100000, 500000)]
        public int Number { get; set; }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesCharArrayInput()
        {
            char[] expected = _data;

            using (var connection = new c())
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                char[] result = connection.Deserialize(serialisedData) as char[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [GlobalSetup]
        public void Setup()
        {
            _data = new char[Number];
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = (char)Math.Abs(new Random(i).Next(0, 127));
            }
        }
    }
}
