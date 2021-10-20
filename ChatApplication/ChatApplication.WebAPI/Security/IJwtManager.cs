using ChatApplication.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ChatApplication.WebAPI.Security
{
    public interface IJwtManager
    {
        ClaimsIdentity CreateSubject(UserWrapper user);

        string GenerateJWTToken(ClaimsIdentity user);

        RefreshTokenWrapper GenerateRefreshToken(string ipAddress);

        SecurityToken ParseToken(string encoded);
    }
}
