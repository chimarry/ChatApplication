using ChatApplication.Core.Options;
using ChatApplication.Core.Util;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.X509;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace ChatApplication.Core.Services
{
    public class CertificateService : ICertificateService
    {
        private const string subjectNameCA = "Chat Issuing Authority";
        private const string cnFormat = "CN={0}";
        private const int caCertIndex = 0;
        private const int uniqueCertCount = 1;

        private static readonly Oid chatCertOid = new Oid("1.3.6.1.5.5.7.3.8");
        private static readonly byte[] serialNumber = new byte[] { 1, 2, 3, 4 };

        private readonly CertificateOptions options;

        public CertificateService(IOptions<CertificateOptions> options)
        {
            this.options = options.Value;
        }

        /// <summary>
        /// Create certificate related to unique username and export 
        /// this newly created certificate in pfx format. 
        /// Created certificate is stored in certificate storage of current user.
        /// </summary>
        /// <param name="username">Unqiue username to identify user for whom certificate is created.</param>
        /// <returns>Exported certificate in pfx format</returns>
        public BasicFileInfo GenerateCertificate(string username)
        {
            X509Certificate2 caCert = GetCACertificate();
            using RSA userRSA = RSA.Create(2048);

            CertificateRequest userCSR = new CertificateRequest(
                string.Format(cnFormat, username),
                userRSA,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);

            userCSR.CertificateExtensions.Add(
                new X509BasicConstraintsExtension(false, false, 0, false));

            userCSR.CertificateExtensions.Add(
                new X509KeyUsageExtension(
                    X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.NonRepudiation,
                    false));

            userCSR.CertificateExtensions.Add(
                new X509EnhancedKeyUsageExtension(
                    new OidCollection { chatCertOid },
                    true));

            userCSR.CertificateExtensions.Add(
                new X509SubjectKeyIdentifierExtension(userCSR.PublicKey, false));

            using X509Certificate2 newUserCert = userCSR.Create(
                caCert,
                DateTimeOffset.UtcNow.AddDays(-1),
                DateTimeOffset.UtcNow.AddDays(90),
                serialNumber);

            X509Certificate2 newUserCertWithPrivateKey = new X509Certificate2(
                newUserCert.Export(X509ContentType.Pkcs12,
                options.UserSecret),
                options.CaSecret,
                X509KeyStorageFlags.PersistKeySet);

            // Save user certificate
            X509Store userStore = new X509Store(StoreName.TrustedPeople, StoreLocation.CurrentUser);
            userStore.Open(OpenFlags.ReadWrite | OpenFlags.OpenExistingOnly);
            userStore.Add(newUserCertWithPrivateKey);

            userStore.Close();

            return new BasicFileInfo()
            {
                FileData = newUserCertWithPrivateKey.Export(X509ContentType.Pfx),
                FileName = string.Format("{0}.pfx", username)
            };
        }

        /// <summary>
        /// Check is certificate is valid.
        /// </summary>
        /// <param name="username">Username for with certificate is checked.</param>
        /// <returns>True if valid, false if not.</returns>
        public bool IsValidCertificate(string username)
        {
            X509Certificate2 caCert = GetCACertificate();
            X509Store userStore = new X509Store(StoreName.TrustedPeople, StoreLocation.CurrentUser);
            userStore.Open(OpenFlags.ReadWrite | OpenFlags.OpenExistingOnly);

            X509Certificate2Collection certificates =
                userStore.Certificates.Find(X509FindType.FindBySubjectName, username, true);

            if (certificates.Count != uniqueCertCount)
                return false;

            X509Certificate2 userCert = certificates[uniqueCertCount - 1];

            try
            {
                Org.BouncyCastle.X509.X509Certificate caCertParsed = new X509CertificateParser().ReadCertificate(caCert.GetRawCertData());
                Org.BouncyCastle.X509.X509Certificate userCertParsed = new X509CertificateParser().ReadCertificate(userCert.GetRawCertData());

                userCertParsed.Verify(caCertParsed.GetPublicKey());
                return userCert.Issuer == string.Format(cnFormat, subjectNameCA) && userCertParsed.IsValidNow;
            }
            catch
            {
                return false;
            }
        }

        public bool IsValidCertificate(string username, byte[] certificate)
        {
            X509Certificate2 caCert = GetCACertificate();
            X509Store userStore = new X509Store(StoreName.TrustedPeople, StoreLocation.CurrentUser);
            userStore.Open(OpenFlags.ReadWrite | OpenFlags.OpenExistingOnly);

            X509Certificate2Collection certificates =
                userStore.Certificates.Find(X509FindType.FindBySubjectName, username, true);

            if (certificates.Count != uniqueCertCount)
                return false;

            X509Certificate2 userCert = certificates[uniqueCertCount - 1];

            try
            {
                Org.BouncyCastle.X509.X509Certificate caCertParsed = new X509CertificateParser().ReadCertificate(caCert.GetRawCertData());
                Org.BouncyCastle.X509.X509Certificate userCertParsed = new X509CertificateParser().ReadCertificate(userCert.GetRawCertData());

                userCertParsed.Verify(caCertParsed.GetPublicKey());
                bool originalCertValidation = userCert.Issuer == string.Format(cnFormat, subjectNameCA) && userCertParsed.IsValidNow;
                var originalCert = new X509Certificate2(certificate);
                return originalCertValidation && ArrayComparer.ByteArraysEqual(originalCert.RawData, userCert.RawData);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Create or find CA certificate identified by specific common name.
        /// </summary>
        /// <returns>CA certificate</returns>
        private X509Certificate2 GetCACertificate()
        {
            X509Certificate2 caCert = null;
            X509Store caStore = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            caStore.Open(OpenFlags.ReadWrite | OpenFlags.OpenExistingOnly);

            X509Certificate2Collection x509Certificate2Collection = caStore.Certificates
                                                                           .Find(X509FindType.FindBySubjectName,
                                                                                 subjectNameCA,
                                                                                 true);
            // Create CA if there is none
            if (x509Certificate2Collection.Count < uniqueCertCount)
            {
                using RSA caRSA = RSA.Create(4096);

                CertificateRequest caCRS = new CertificateRequest(
                    string.Format(cnFormat, subjectNameCA),
                    caRSA,
                    HashAlgorithmName.SHA256,
                    RSASignaturePadding.Pkcs1);

                caCRS.CertificateExtensions.Add(
                    new X509BasicConstraintsExtension(true, false, 0, true));

                caCRS.CertificateExtensions.Add(
                    new X509SubjectKeyIdentifierExtension(caCRS.PublicKey, false));
                using X509Certificate2 newCACert = caCRS.CreateSelfSigned(
                    DateTimeOffset.UtcNow.AddDays(-45),
                    DateTimeOffset.UtcNow.AddDays(365));

                X509Certificate2 withPrivateKey = new X509Certificate2(
                    newCACert.Export(X509ContentType.Pkcs12, options.UserSecret),
                    options.CaSecret,
                    X509KeyStorageFlags.MachineKeySet);
                caStore.Add(withPrivateKey);
                caCert = withPrivateKey;
            }
            else
                caCert = x509Certificate2Collection[caCertIndex];
            caStore.Close();
            return caCert;
        }
    }
}
