namespace ChatApplication.WebAPI.Models
{
    public class AuthenticateCredentialsResponseWrapper
    {
        public string OtpApiKey { get; }

        public AuthenticateCredentialsResponseWrapper(string otpApiKey)
        {
            OtpApiKey = otpApiKey;
        }
    }
}
