using ChatApplication.Core.ErrorHandling;
using ChatApplication.WebAPI.Models;
using ChatApplication.WebAPI.Util;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ChatApplication.Core.Options;

namespace ChatApplication.WebAPI.Security
{
    public class JwtManager : IJwtManager
    {
        private readonly AuthenticationOptions options;

        public JwtManager(IOptions<AuthenticationOptions> options)
        {
            this.options = options.Value;
        }

        public ClaimsIdentity CreateSubject(UserWrapper user)
            => new ClaimsIdentity(new Claim[]
                   {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
                   });

        public string GenerateJWTToken(ClaimsIdentity subject)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(options.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = DateTime.UtcNow.AddMinutes(options.AccessTokenLifetimeMin),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public RefreshTokenWrapper GenerateRefreshToken(string ipAddress)
        {
            using RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return new RefreshTokenWrapper
            {
                Token = Convert.ToBase64String(randomBytes),
                ExpiresOn = DateTime.UtcNow.AddDays(options.RefreshTokenLifetimeDay),
                CreatedOn = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        public SecurityToken ParseToken(string bearerToken)
        {
            try
            {
                if (!bearerToken.StartsWith(HttpConstants.Bearer))
                    throw new UnauthorizedException();
                string token = bearerToken.Remove(0, HttpConstants.Bearer.Length);
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Secret)),
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true
                }, out SecurityToken validatedToken);

                return validatedToken;
            }
            catch
            {
                return null;
            }
        }
    }
}
