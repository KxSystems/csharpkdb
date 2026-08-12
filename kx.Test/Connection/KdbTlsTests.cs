using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;

namespace kx.Test.Connection
{
    [TestFixture]
    public class KdbTlsTests
    {
        [Test]
        public void DefaultReturnsExpectedTlsOptions()
        {
            const string targetHost = "localhost";

            KdbTlsOptions options = KdbTls.Default(targetHost);

            Assert.IsTrue(options.Enabled);
            Assert.AreEqual(targetHost, options.TargetHost);
            Assert.IsNull(options.RemoteCertificateValidationCallback);
        }

        [Test]
        public void IgnoreHostnameMismatchAcceptsNoCertificateErrors()
        {
            KdbTlsOptions options =
                KdbTls.IgnoreHostnameMismatch("localhost");

            bool result = options.RemoteCertificateValidationCallback(
                null,
                null,
                null,
                SslPolicyErrors.None);

            Assert.IsTrue(result);
        }

        [Test]
        public void IgnoreHostnameMismatchAcceptsHostnameMismatch()
        {
            KdbTlsOptions options =
                KdbTls.IgnoreHostnameMismatch("localhost");

            bool result = options.RemoteCertificateValidationCallback(
                null,
                null,
                null,
                SslPolicyErrors.RemoteCertificateNameMismatch);

            Assert.IsTrue(result);
        }

        [Test]
        public void IgnoreHostnameMismatchStillRejectsChainErrorsWhenNameAlsoMismatches()
        {
            KdbTlsOptions options =
                KdbTls.IgnoreHostnameMismatch("localhost");

            SslPolicyErrors errors =
                SslPolicyErrors.RemoteCertificateNameMismatch |
                SslPolicyErrors.RemoteCertificateChainErrors;

            bool result = options.RemoteCertificateValidationCallback(
                null,
                null,
                null,
                errors);

            Assert.IsFalse(result);
        }

        [Test]
        public void IgnoreHostnameMismatchRejectsMissingCertificate()
        {
            KdbTlsOptions options =
                KdbTls.IgnoreHostnameMismatch("localhost");

            bool result = options.RemoteCertificateValidationCallback(
                null,
                null,
                null,
                SslPolicyErrors.RemoteCertificateNotAvailable);

            Assert.IsFalse(result);
        }

        [Test]
        public void InsecureAcceptsAllCertificateErrors()
        {
            KdbTlsOptions options =
                KdbTls.Insecure("localhost");

            SslPolicyErrors errors =
                SslPolicyErrors.RemoteCertificateNameMismatch |
                SslPolicyErrors.RemoteCertificateChainErrors |
                SslPolicyErrors.RemoteCertificateNotAvailable;

            bool result = options.RemoteCertificateValidationCallback(
                null,
                null,
                null,
                errors);

            Assert.IsTrue(result);
        }

        [Test]
        public void DisabledHasTlsDisabled()
        {
            KdbTlsOptions options = KdbTlsOptions.Disabled;

            Assert.IsFalse(options.Enabled);
        }

        [Test]
        public void KdbTlsOptionsHasExpectedDefaults()
        {
            KdbTlsOptions options = new KdbTlsOptions();

            Assert.IsFalse(options.Enabled);
            Assert.IsNull(options.TargetHost);
            Assert.IsNotNull(options.ClientCertificates);
            Assert.AreEqual(0, options.ClientCertificates.Count);
            Assert.IsNull(options.EnabledSslProtocols);
            Assert.IsNull(options.CertificateRevocationCheckMode);
            Assert.IsNull(options.RemoteCertificateValidationCallback);
            Assert.IsNull(options.LocalCertificateSelectionCallback);
        }

        [Test]
        public void KdbTlsOptionsRetainsConfiguredValues()
        {
            RemoteCertificateValidationCallback remoteCallback =
                (sender, certificate, chain, errors) => true;

            LocalCertificateSelectionCallback localCallback =
                (sender, targetHost, localCertificates,
                    remoteCertificate, acceptableIssuers) => null;

            KdbTlsOptions options = new KdbTlsOptions
            {
                Enabled = true,
                TargetHost = "test.example.com",
                EnabledSslProtocols = SslProtocols.Tls12,
                CertificateRevocationCheckMode = X509RevocationMode.Offline,
                RemoteCertificateValidationCallback = remoteCallback,
                LocalCertificateSelectionCallback = localCallback
            };

            Assert.IsTrue(options.Enabled);
            Assert.AreEqual("test.example.com", options.TargetHost);
            Assert.AreEqual(
                SslProtocols.Tls12,
                options.EnabledSslProtocols);
            Assert.AreEqual(
                X509RevocationMode.Offline,
                options.CertificateRevocationCheckMode);
            Assert.AreSame(
                remoteCallback,
                options.RemoteCertificateValidationCallback);
            Assert.AreSame(
                localCallback,
                options.LocalCertificateSelectionCallback);
        }
    }
}
