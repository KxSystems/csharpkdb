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
                Logger.Info($"Connecting to {host}:{port}");
                connection = new c(host, port, usernamePassword);

                object result = connection.k("2+3");
                Logger.Info($"Result of 2+3:{result}");

                result = connection.k("type",true);
                Logger.Info($"q type for c# bool:{result}");

                result = connection.k("type",Guid.NewGuid());
                Logger.Info($"q type for c# Guid:{result}");
                
                result = connection.k("type",(byte)1);
                Logger.Info($"q type for c# byte:{result}");

                result = connection.k("type",(short)1);
                Logger.Info($"q type for c# short:{result}");

                result = connection.k("type",22);
                Logger.Info($"q type for c# int:{result}");

                result = connection.k("type",22L);
                Logger.Info($"q type for c# long:{result}");

                result = connection.k("type",22.2F);
                Logger.Info($"q type for c# float:{result}");

                result = connection.k("type",22.2D);
                Logger.Info($"q type for c# double:{result}");

                result = connection.k("type",'a');
                Logger.Info($"q type for c# char:{result}");

                result = connection.k("type","hello");
                Logger.Info($"q type for c# string:{result}");

                result = connection.k("type",new System.DateTime(1999, 1, 13, 3, 57, 32, 11));
                Logger.Info($"q type for c# DateTime:{result}");

                result = connection.k("type",new c.Month(1));
                Logger.Info($"q type for c# c.Month:{result}");

                result = connection.k("type",new c.Date(1));
                Logger.Info($"q type for c# c.Date:{result}");

                result = connection.k("type",new c.KTimespan(1));
                Logger.Info($"q type for c# c.KTimespan:{result}");

                result = connection.k("type",new c.Minute(1));
                Logger.Info($"q type for c# c.Minute:{result}");

                result = connection.k("type",new c.Second(1));
                Logger.Info($"q type for c# c.Second:{result}");

                result = connection.k("type",new TimeSpan(1));
                Logger.Info($"q type for c# TimeSpan:{result}");

                result = connection.k("type","hello".ToCharArray());
                Logger.Info($"q type for c# character array:{result}");
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
