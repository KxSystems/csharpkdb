using System;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace kx
{
    /// <summary>
    /// Defines TLS/SSL options for a KDB+ client connection.
    /// </summary>
    public sealed class KdbTlsOptions
    {
        /// <summary>
        /// Gets a disabled TLS configuration.
        /// </summary>
        public static KdbTlsOptions Disabled { get; } = new KdbTlsOptions
        {
            Enabled = false
        };

        /// <summary>
        /// Gets or sets a value indicating whether TLS is enabled for the connection.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the target host name used during TLS authentication.
        /// </summary>
        /// <remarks>
        /// This value is typically used for server certificate name validation.
        /// </remarks>
        public string TargetHost { get; set; }

        /// <summary>
        /// Gets the client certificates to present to the server during TLS authentication.
        /// </summary>
        public X509CertificateCollection ClientCertificates { get; } = new X509CertificateCollection();

        /// <summary>
        /// Gets or sets the allowed SSL/TLS protocol versions.
        /// </summary>
        public SslProtocols? EnabledSslProtocols { get; set; }

        /// <summary>
        /// Gets or sets the certificate revocation checking mode to use during TLS authentication.
        /// </summary>
        public X509RevocationMode? CertificateRevocationCheckMode { get; set; }

        /// <summary>
        /// Gets or sets the callback used to validate the remote server certificate.
        /// </summary>
        public RemoteCertificateValidationCallback RemoteCertificateValidationCallback { get; set; }

        /// <summary>
        /// Gets or sets the callback used to select a local client certificate.
        /// </summary>
        public LocalCertificateSelectionCallback LocalCertificateSelectionCallback { get; set; }

        /// <summary>
        /// Gets or sets an optional callback used to configure
        /// <see cref="SslClientAuthenticationOptions"/> before authentication.
        /// </summary>
        public Action<SslClientAuthenticationOptions> ConfigureAuthenticationOptions { get; set; }
    }
}