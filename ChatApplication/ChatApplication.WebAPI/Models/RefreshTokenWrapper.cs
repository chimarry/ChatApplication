using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApplication.WebAPI.Models
{
    public class RefreshTokenWrapper
    {
        public int UserId { get; set; }

        public string Token { get; set; }

        public DateTime ExpiresOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedByIp { get; set; }

        public DateTime? RevokedOn { get; set; }

        public string RevokedByIp { get; set; }

        public string ReplacedByToken { get; set; }
    }
}
