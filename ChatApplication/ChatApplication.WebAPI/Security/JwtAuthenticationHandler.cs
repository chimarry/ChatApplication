using ChatApplication.Core.ErrorHandling;
using ChatApplication.Core.Services;
using ChatApplication.WebAPI.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ChatApplication.WebAPI.Security
{
    public class JwtAuthenticationHandler : AuthenticationHandler<JwtAuthenticationOptions>
    {
        private IJwtManager jwtManager;

        public JwtAuthenticationHandler(IOptionsMonitor<JwtAuthenticationOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IJwtManager jwtManager)
            : base(options, logger, encoder, clock)
        {
            this.jwtManager = jwtManager;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string encodedAccessToken = Request.GetAuthorizationHeader();

            if (!(jwtManager.ParseToken(encodedAccessToken) is JwtSecurityToken parsedToken) || parsedToken.ValidTo <= DateTime.UtcNow)
                throw new UnauthorizedException();

            Claim[] claims = new[]
            {
                 new Claim(ClaimTypes.UserData, encodedAccessToken),
                 new Claim(ClaimTypes.Name, parsedToken.Claims.FirstOrDefault(x => x.Type==HttpConstants.ClaimName).Value)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, Authentication.JwtScheme);
            AuthenticationTicket ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties(),
                Authentication.JwtScheme);
            return AuthenticateResult.Success(ticket);
        }
    }
}
