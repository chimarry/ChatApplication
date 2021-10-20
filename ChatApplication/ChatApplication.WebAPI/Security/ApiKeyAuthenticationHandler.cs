using ChatApplication.Core.ErrorHandling;
using ChatApplication.WebAPI.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ChatApplication.WebAPI.Security
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private readonly string apiKey;

        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IOptions<ApiKeyOptions> apiKeyOptions)
            : base(options, logger, encoder, clock)
        {
            this.apiKey = apiKeyOptions.Value.ApiKey;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(HttpConstants.ApiKey))
                throw new UnauthorizedException();

            string apiKey = Request.Headers[HttpConstants.ApiKey];

            if (apiKey != this.apiKey)
                throw new UnauthorizedException();

            Claim[] claims = new[]
            {
                 new Claim(ClaimTypes.Anonymous, apiKey)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, Authentication.ApiKeyScheme);
            AuthenticationTicket ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties(),
                Authentication.ApiKeyScheme);
            return AuthenticateResult.Success(ticket);
        }
    }
}
