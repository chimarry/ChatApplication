using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ChatApplication.Core.Util
{
    public class EmailSender : IEmailSender
    {
        private const string username = "xxxxxx@gmail.com";
        private const string password = "xxxxxxxxxx";
        private const string host = "smtp.gmail.com";
        private const int port = 587;

        private const string otpMessageFormat = "Your OTP code is: {0}";
        private const string otpSubject = "OTP code - OkRam";
        private const string certSubject = "Certificate - OkRam";
        private const string certMessage = "Your certificate is in attachment";

        public async Task SendCertificate(BasicFileInfo certificate, string email)
            => await SendEmail(email, certSubject, certMessage, certificate);

        public async Task SendOTP(string otp, string email)
            => await SendEmail(email, otpSubject, string.Format(otpMessageFormat, otp));

        private async Task SendEmail(string email, string subject, string content, BasicFileInfo certificate = null)
        {
            using SmtpClient smtpClient = new SmtpClient();
            NetworkCredential basicCredential = new NetworkCredential(username, password);
            using MailMessage message = new MailMessage();
            MailAddress fromAddress = new MailAddress(username);

            smtpClient.Host = host;
            smtpClient.Port = port;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = basicCredential;
            smtpClient.EnableSsl = true;

            message.From = fromAddress;
            message.Subject = subject;
            message.IsBodyHtml = false;
            message.Body = content;
            message.To.Add(email);

            if (certificate != null)
            {
                Attachment attachment = new Attachment(new MemoryStream(certificate.FileData), certificate.FileName, "application/x-pkcs12");
                message.Attachments.Add(attachment);
            }

            try
            {
                await smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
