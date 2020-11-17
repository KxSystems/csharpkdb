using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace kx.Test.TestUtils
{
    /// <summary>
    /// A wrapper over a TCP Listener to simulate a running KDB+ environment for which
    /// client log-in will succeed.
    /// </summary>
    internal sealed class TestableTcpServer : IDisposable
    {
        private readonly TcpListener _server;

        /// <summary>
        /// Initialises a new default instance of <see cref="TestableTcpServer"/>.
        /// </summary>
        public TestableTcpServer()
        {
            _server = new TcpListener(IPAddress.Loopback, 0);
            _server.Start();

            //kick off task to simulate successful log-on
            Task.Factory.StartNew(() =>
            {
                //block until client has established connection
                if (!_server.Pending())
                {
                    System.Threading.Thread.Sleep(10);
                }

                //now get the stream from the client
                var client = _server.AcceptTcpClient();
                var stream = client.GetStream();

                byte[] buffer = new byte[client.ReceiveBufferSize];

                // read incoming stream
                stream.Read(buffer, 0, client.ReceiveBufferSize);

                //simulate authentication succeeding
                stream.Write(buffer, 0, 1);
            });
        }

        /// <summary>
        /// Gets the port this class is connected to for testing.
        /// </summary>
        public int TestPort
        {
            get
            {
                return ((IPEndPoint)_server.LocalEndpoint).Port;
            }
        }

        #region IDisposable Members
        public void Dispose()
        {
            if (_server != null)
            {
                _server.Stop();
            }
        }
        #endregion IDisposable Members
    }
}
