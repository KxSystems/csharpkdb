using System.Net.Security;

namespace kx
{
    /// <summary>
    /// Provides helper methods for creating common TLS configurations
    /// for KDB+ client connections.
    /// </summary>
    public static class KdbTls
    {
        /// <summary>
        /// Creates a TLS configuration that uses the default platform
        /// certificate validation behaviour.
        /// </summary>
        /// <param name="targetHost">
        /// The target host name used during TLS authentication.
        /// </param>
        /// <returns>
        /// A <see cref="KdbTlsOptions"/> instance configured for standard TLS validation.
        /// </returns>
        public static KdbTlsOptions Default(string targetHost) =>
            new KdbTlsOptions
            {
                Enabled = true,
                TargetHost = targetHost
            };

        /// <summary>
        /// Creates a TLS configuration that ignores certificate host name mismatch
        /// errors while still rejecting other certificate validation failures.
        /// </summary>
        /// <param name="targetHost">
        /// The target host name used during TLS authentication.
        /// </param>
        /// <returns>
        /// A <see cref="KdbTlsOptions"/> instance configured to ignore
        /// <see cref="SslPolicyErrors.RemoteCertificateNameMismatch"/>.
        /// </returns>
        public static KdbTlsOptions IgnoreHostnameMismatch(string targetHost) =>
            new KdbTlsOptions
            {
                Enabled = true,
                TargetHost = targetHost,
                RemoteCertificateValidationCallback = (sender, certificate, chain, errors) =>
                {
                    var filteredErrors = errors & ~SslPolicyErrors.RemoteCertificateNameMismatch;
                    return filteredErrors == SslPolicyErrors.None;
                }
            };

        /// <summary>
        /// Creates a TLS configuration that accepts any server certificate.
        /// </summary>
        /// <param name="targetHost">
        /// The target host name used during TLS authentication.
        /// </param>
        /// <returns>
        /// A <see cref="KdbTlsOptions"/> instance configured to accept all server certificates.
        /// </returns>
        /// <remarks>
        /// This disables certificate validation and should only be used in controlled
        /// environments where the security implications are understood.
        /// </remarks>
        public static KdbTlsOptions Insecure(string targetHost) =>
#pragma warning disable CA5359
            new KdbTlsOptions
            {
                Enabled = true,
                TargetHost = targetHost,
                RemoteCertificateValidationCallback = (sender, certificate, chain, errors) => true
            };
#pragma warning restore CA5359
    }
}