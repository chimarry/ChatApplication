using ChatApplication.Core.AutoMapper;
using ChatApplication.Core.DTO;
using ChatApplication.Core.Entities;
using ChatApplication.Core.ErrorHandling;
using ChatApplication.Core.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ChatApplication.Core.Services
{
    public class UserService : IUserService
    {
        private readonly ICertificateService certificateService;
        private readonly IEmailSender emailSender;
        private readonly ChatDbContext context;

        public UserService(ICertificateService certificateService, IEmailSender emailSender, ChatDbContext context)
        {
            this.context = context;
            this.certificateService = certificateService;
            this.emailSender = emailSender;
        }

        public async Task<ResultMessage<string>> AuthenticateWithCredentials(CredentialsDTO dto, BasicFileInfo certificate)
        {
            // Find user
            User user = await context.Users.SingleOrDefaultAsync(x => x.Username == dto.Username);
            if (user == null)
                return new ResultMessage<string>(OperationStatus.InvalidData);

            // Validate password
            if (!PasswordGenerator.ConfirmPassword(dto.Password, user.Salt, user.Password))
                return new ResultMessage<string>(OperationStatus.InvalidData);

            // Validate certificate
            if (!certificateService.IsValidCertificate(dto.Username, certificate.FileData))
                return new ResultMessage<string>(OperationStatus.InvalidData);

            Random random = new Random();
            user.Otp = random.Next(100000, 999999).ToString();
            user.OtpExpiresOn = DateTime.UtcNow.AddMinutes(5);
            user.OtpApiKey = Guid.NewGuid().ToString();

            await context.SaveChangesAsync();

            await emailSender.SendOTP(user.Otp, user.Email);

            return new ResultMessage<string>(user.OtpApiKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otp"></param>
        /// <param name="otpApiKey"></param>
        /// <exception cref="UnauthorizedException"></exception>
        /// <returns></returns>
        public async Task<ResultMessage<OutputUserDTO>> AuthenticateWithOTP(string otp, string otpApiKey)
        {
            User user = await context.Users
                                     .SingleOrDefaultAsync(x => x.Otp == otp && otpApiKey == x.OtpApiKey);

            if (user != null && user.OtpExpiresOn.HasValue && DateTime.SpecifyKind(user.OtpExpiresOn.Value, DateTimeKind.Utc) > DateTime.UtcNow)
            {
                user.Otp = null;
                user.OtpApiKey = null;
                user.OtpExpiresOn = null;
                OutputUserDTO dto = Mapping.Mapper.Map<OutputUserDTO>(user);
                return new ResultMessage<OutputUserDTO>(dto);
            }

            throw new UnauthorizedException();
        }

        public async Task<ResultMessage<bool>> Register(UserDTO userDTO)
        {
            if (!AreValidCredentials(userDTO.Credentials) || !InputValidator.IsValidEmail(userDTO.Email))
                return new ResultMessage<bool>(OperationStatus.InvalidData);

            if (await context.Users.AnyAsync(x => x.Email == userDTO.Email ||
                                                  x.Username.Equals(userDTO.Credentials.Username, StringComparison.OrdinalIgnoreCase)))
                return new ResultMessage<bool>(OperationStatus.Exists);

            BasicFileInfo certificate = certificateService.GenerateCertificate(userDTO.Credentials.Username);
            if (certificate != null)
                await emailSender.SendCertificate(certificate, userDTO.Email);

            User user = Mapping.Mapper.Map<User>(userDTO);
            (user.Salt, user.Password) = PasswordGenerator.ComputePassword(userDTO.Credentials.Password);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return new ResultMessage<bool>(true);
        }

        public bool AreValidCredentials(CredentialsDTO credentials)
             => InputValidator.IsValidUsername(credentials.Username)
                && InputValidator.IsValidPassword(credentials.Password);

        public async Task<bool> Exists(string otpApiKey)
            => await context.Users.AnyAsync(x => x.OtpApiKey == otpApiKey);
    }
}
