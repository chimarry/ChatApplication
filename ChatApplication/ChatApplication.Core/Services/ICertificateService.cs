using ChatApplication.Core.Util;

namespace ChatApplication.Core.Services
{
    public interface ICertificateService
    {
        BasicFileInfo GenerateCertificate(string username);

        bool IsValidCertificate(string username);

        bool IsValidCertificate(string username, byte[] certificate);
    }
}
