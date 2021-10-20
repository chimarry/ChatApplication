using System;
using System.Collections.Generic;

namespace ChatApplication.Core.Entities
{
    public class User
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public byte[] Password { get; set; }

        public byte[] Salt { get; set; }

        public string Otp { get; set; }

        public string OtpApiKey { get; set; }

        public DateTime? OtpExpiresOn { get; set; }

        public ICollection<Message> SentMessages { get; set; }

        public ICollection<Message> ReceivedMessages { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
