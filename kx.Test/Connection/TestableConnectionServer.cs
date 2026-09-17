using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace kx.Test.TestUtils
{
    internal sealed class TestableIpv6Server : IDisposable
    {
        private readonly TcpListener _listener;
        private readonly Task _serverTask;

        internal TestableIpv6Server()
        {
            _listener = new TcpListener(IPAddress.IPv6Loopback, 0);
            _listener.Server.DualMode = false;
            try
            {
                _listener.Start();
            }
            catch
            {
                _listener.Stop();
                throw;
            }
            _serverTask = Task.Run(() =>
            {
                using (TcpClient client = _listener.AcceptTcpClient())
                {
                    TestableConnectionServer.PerformKdbHandshake(client.GetStream());
                }
            });
        }

        internal int Port => ((IPEndPoint)_listener.LocalEndpoint).Port;

        public void Dispose()
        {
            _listener.Stop();
            TestableConnectionServer.WaitForServer(_serverTask);
        }
    }

#if NETCOREAPP3_1_OR_GREATER
    internal sealed class TestableUnixDomainSocketServer : IDisposable
    {
        private readonly string _path;
        private readonly Socket _listener;
        private readonly Task _serverTask;

        internal TestableUnixDomainSocketServer()
        {
            _path = Path.Combine(Path.GetTempPath(), $"csharpkdb-{Guid.NewGuid():N}.sock");
            _listener = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);
            _listener.Bind(new UnixDomainSocketEndPoint(_path));
            _listener.Listen(1);
            _serverTask = Task.Run(() =>
            {
                using (Socket socket = _listener.Accept())
                using (var stream = new NetworkStream(socket, ownsSocket: false))
                {
                    TestableConnectionServer.PerformKdbHandshake(stream);
                }
            });
        }

        internal string SocketPath => _path;

        public void Dispose()
        {
            _listener.Dispose();
            TestableConnectionServer.WaitForServer(_serverTask);
            if (File.Exists(_path))
            {
                File.Delete(_path);
            }
        }
    }

    internal sealed class TestableTlsServer : IDisposable
    {
        private readonly TcpListener _listener;
        private readonly RSA _key;
        private readonly X509Certificate2 _certificate;
        private readonly Task _serverTask;
        private volatile TcpClient _acceptedClient;

        internal TestableTlsServer(bool allowClientRejection = false)
        {
            _key = RSA.Create(2048);
            var request = new CertificateRequest(
                "CN=localhost",
                _key,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);
            request.CertificateExtensions.Add(
                new X509BasicConstraintsExtension(false, false, 0, false));
            request.CertificateExtensions.Add(
                new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, false));
            var names = new SubjectAlternativeNameBuilder();
            names.AddDnsName("localhost");
            request.CertificateExtensions.Add(names.Build());
            _certificate = request.CreateSelfSigned(
                DateTimeOffset.UtcNow.AddMinutes(-5),
                DateTimeOffset.UtcNow.AddDays(1));

            _listener = new TcpListener(IPAddress.Loopback, 0);
            _listener.Start();
            _serverTask = Task.Run(() =>
            {
                try
                {
                    using (TcpClient client = _listener.AcceptTcpClient())
                    using (var stream = new SslStream(client.GetStream(), false))
                    {
                        _acceptedClient = client;
                        stream.AuthenticateAsServer(
                            _certificate,
                            false,
                            SslProtocols.Tls12,
                            false);
                        TestableConnectionServer.PerformKdbHandshake(stream);
                    }
                }
                catch when (allowClientRejection)
                {
                    // Expected when default validation rejects the self-signed certificate.
                }
            });
        }

        internal int Port => ((IPEndPoint)_listener.LocalEndpoint).Port;

        public void Dispose()
        {
            _listener.Stop();
            _acceptedClient?.Close();
            TestableConnectionServer.WaitForServer(_serverTask);
            _certificate.Dispose();
            _key.Dispose();
        }
    }
#endif

    internal static class TestableConnectionServer
    {
        internal static void PerformKdbHandshake(Stream stream)
        {
            var buffer = new byte[256];
            if (stream.Read(buffer, 0, buffer.Length) == 0)
            {
                throw new IOException("The client closed before sending the KDB+ handshake.");
            }
            stream.WriteByte(3);
            stream.Flush();
        }

        internal static void WaitForServer(Task serverTask)
        {
            if (!serverTask.Wait(TimeSpan.FromSeconds(5)))
            {
                throw new TimeoutException("The test server did not complete.");
            }
        }
    }
}
