using System.Linq;
using BenchmarkDotNet.Attributes;
using NUnit.Framework;

namespace kx.Benchmark.Test.Connection
{
    [MemoryDiagnoser]
    [MedianColumn]
    [MaxColumn]
    public class ConnectionByteArraySerialisationTests
    {
        private byte[] _data;

        [Params(1000, 10000, 100000, 500000)]
        public int Number { get; set; }

        [Benchmark]
        public void ConnectionSerialisesAndDeserialisesByteArrayInput()
        {
            byte[] expected = _data;

            using (var connection = new c())
            {
                byte[] serialisedData = connection.Serialize(1, expected);

                byte[] result = connection.Deserialize(serialisedData) as byte[];

                Assert.IsTrue(Enumerable.SequenceEqual(expected, result));
            }
        }

        [GlobalSetup]
        public void Setup()
        {
            _data = new byte[Number];
            for(int i = 0; i < _data.Length; i++)
            {
                _data[i] = 0x20;
            }
        }
    }
}
