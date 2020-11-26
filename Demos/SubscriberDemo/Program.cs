using System;
using System.Threading.Tasks;
using kx;
using NLog;

namespace SubscriberDemo
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

                //open subscription
                connection.ks(".u.sub", "mytable", "MSFT");

                bool subscribing = true;

                //start processing subscriptions until user exit or error
                Task.Factory.StartNew(() =>
                {
                    Logger.Info("Processing subscription results. Press any key to exit");
                    while (subscribing)
                    {
                        try
                        {
                            Logger.Info($"Received subscription result:{connection.k()}");
                        }
                        catch (Exception)
                        {
                            Logger.Error("Error occurred processing Subscription. Exiting Subscription-Demo");
                            subscribing = false;
                        }
                    }
                });

                Console.ReadLine();
                subscribing = false;

            }
            catch (Exception ex)
            {
                Logger.Error($"Error occurred running Subscription-Demo. \r\n{ex}");
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }
    }
}
