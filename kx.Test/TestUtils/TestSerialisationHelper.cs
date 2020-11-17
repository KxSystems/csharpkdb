using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace kx.Test.TestUtils
{
    /// <summary>
    /// A helper class for testing serialisation and de-serialisation logic 
    /// in unit-tests
    /// </summary>
    internal static class TestSerialisationHelper
    {
        /// <summary>
        /// Performs binary serialisation and de-serialisation on a specified exception.
        /// </summary>
        /// <typeparam name="T">The type of exception being tested.</typeparam>
        /// <param name="exception">The exception to be serialised and de-serialised.</param>
        /// <returns>
        /// A de-serialised instance of the exception passed. All serialisable members should match the original.
        /// </returns>
        /// <remarks>
        /// This is primarily intended to confirm custom exceptions within the DeltaApiCore 
        /// library comply to ISerialization pattern.
        /// 
        /// See https://stackoverflow.com/questions/94488/what-is-the-correct-way-to-make-a-custom-net-exception-serializable
        /// </remarks>
        public static T SerialiseAndDeserialiseException<T>(T exception)
            where T : Exception
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (var stream = new MemoryStream())
            {
                binaryFormatter.Serialize(stream, exception);

                stream.Seek(0, 0);

                return (T)binaryFormatter.Deserialize(stream);
            }
        }
    }
}
