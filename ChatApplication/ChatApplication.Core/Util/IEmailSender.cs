using System.Threading.Tasks;

namespace ChatApplication.Core.Util
{
    public interface IEmailSender
    {
        Task SendOTP(string otp, string email);

        Task SendCertificate(BasicFileInfo certificate, string email);
    }
}
