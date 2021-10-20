using System.ComponentModel.DataAnnotations;

namespace ChatApplication.WebAPI.Models
{
    public class RegisterUserPostWrapper
    {
        /// <summary>
        /// Username
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
