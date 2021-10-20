using System.ComponentModel.DataAnnotations;

namespace ChatApplication.WebAPI.Models
{
    public class AuthenticateOtpPostWrapper
    {
        /// <summary>
        /// OTP code
        /// </summary>
        [Required]
        public string Otp { get; set; }
    }
}
