using System;
using kx;
using NLog;

namespace SerializationDemo
{
    static class Program
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        static void Main()
        {
            string host = "localhost";
            int port = 5001;
            string usernamePassword = $"{Environment.UserName}:mypassword";

            c connection = null;

            int[] input = CreateTestArray(i => i % 10, 50000);

            try
            {
                connection = new c(host, port, usernamePassword);

                int[] deserialisedResult =
                    connection.Deserialize(connection.Serialize(1, input)) as int[];

                Logger.Info("{0}", System.Linq.Enumerable.SequenceEqual(input, deserialisedResult));
            }
            catch (Exception ex)
            {
                Logger.Error($"Error occurred running Serialization-Demo. \r\n{ex}");
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        private static T[] CreateTestArray<T>(Func<int, T> elementBuilder, int arraySize)
        {
            T[] array = new T[arraySize];

            for (int i = 0; i < arraySize; i++)
            {
                array[i] = elementBuilder(i);
            }
            return array;
        }
    }
}
