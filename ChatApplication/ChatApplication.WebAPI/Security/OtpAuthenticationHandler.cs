using ChatApplication.Core.ErrorHandling;
using ChatApplication.Core.Services;
using ChatApplication.WebAPI.Models;
using ChatApplication.WebAPI.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ChatApplication.WebAPI.Security
{
    public class OtpAuthenticationHandler : AuthenticationHandler<OtpAuthenticationOptions>
    {
        private readonly IUserService userService;

        public OtpAuthenticationHandler(IOptionsMonitor<OtpAuthenticationOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IUserService userService)
            : base(options, logger, encoder, clock)
        {
            this.userService = userService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.RouteValues.ContainsKey(HttpConstants.OtpApiKey))
                throw new UnauthorizedException();
            string otpApiKey = Request.RouteValues[HttpConstants.OtpApiKey].ToString();

            if (!await userService.Exists(otpApiKey))
                throw new UnauthorizedException();

            Claim[] claims = new[]
            {
                 new Claim(ClaimTypes.Anonymous, otpApiKey)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, Authentication.OtpApiKeyScheme);
            AuthenticationTicket ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties(), Authentication.OtpApiKeyScheme);
            return AuthenticateResult.Success(ticket);
        }
    }
}
