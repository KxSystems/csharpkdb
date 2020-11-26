using System;
using kx;
using NLog;

namespace QueryResponseDemo
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
            try
            {
                connection = new c(host, port, usernamePassword);

                object result = connection.k("2+3");

                Logger.Info($"Received result:{result}");

            }
            catch(Exception ex)
            {
                Logger.Error($"Error occurred running QueryResponse-Demo. \r\n{ex}");
            }
            finally
            {
                if(connection != null)
                {
                    connection.Close();
                }
            }
        }
    }
}
