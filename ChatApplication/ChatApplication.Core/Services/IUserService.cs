using ChatApplication.Core.DTO;
using ChatApplication.Core.ErrorHandling;
using ChatApplication.Core.Util;
using System.Threading.Tasks;

namespace ChatApplication.Core.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Enable user to register.
        /// </summary>
        /// <returns></returns>
        Task<ResultMessage<bool>> Register(UserDTO user);

        /// <summary>
        /// Enable user to authenticate with credentials.
        /// This is the first phase of two-factor authentication.
        /// </summary>
        /// <returns></returns>
        Task<ResultMessage<string>> AuthenticateWithCredentials(CredentialsDTO dto, BasicFileInfo certificateData);

        /// <summary>
        /// Enable user to authenticate with otp.
        /// This is the second phase of two-factor authentication.
        /// </summary>
        /// <returns></returns>
        Task<ResultMessage<OutputUserDTO>> AuthenticateWithOTP(string otp, string otpApiKey);

        /// <summary>
        /// Checks if otpApiKey exist in database.
        /// </summary>
        /// <param name="otpApiKey">Value to check</param>
        /// <returns></returns>
        Task<bool> Exists(string otpApiKey);
    }
}
