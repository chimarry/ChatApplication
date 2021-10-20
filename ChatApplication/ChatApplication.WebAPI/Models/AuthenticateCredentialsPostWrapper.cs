using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ChatApplication.WebAPI.Models
{
    public class AuthenticateCredentialsPostWrapper
    {
        /// <summary>
        /// Username
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Certificate
        /// </summary>
        [Required]
        public IFormFile Certificate { get; set; }
    }
}
