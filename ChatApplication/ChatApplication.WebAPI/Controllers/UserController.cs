using ChatApplication.Core.DTO;
using ChatApplication.Core.ErrorHandling;
using ChatApplication.Core.Services;
using ChatApplication.WebAPI.AutoMapper;
using ChatApplication.WebAPI.ErrorHandling;
using ChatApplication.WebAPI.Models;
using ChatApplication.WebAPI.Security;
using ChatApplication.WebAPI.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace ChatApplication.WebAPI.Controllers
{
    /// <summary>
    /// Enpoints for working with users
    /// </summary>
    [Route("users")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Authentication.ApiKeyScheme)]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IRefreshTokenService refreshTokenService;
        private readonly IJwtManager jwtManager;

        public UserController(IUserService userService, IRefreshTokenService refreshTokenService, IJwtManager jwtManager)
        {
            this.userService = userService;
            this.refreshTokenService = refreshTokenService;
            this.jwtManager = jwtManager;
        }

        /// <summary>
        /// Enables user to register. Certificate is generated and sent on user's email.
        /// </summary> 200 204 422 401 429 409 500
        /// <param name="userInfo">Information neccessary for registration process</param>
        /// <returns></returns> ResultMessage<RegisterUserResponseWrapper>
        [HttpPost()]
        [SwaggerResponse(StatusCodes.Status200OK, "The results are returned.", typeof(RegisterUserResponseWrapper))]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserPostWrapper userInfo)
        {
            UserDTO user = Mapping.Mapper.Map<UserDTO>(userInfo);
            ResultMessage<bool> userRegistered = await userService.Register(user);
            return HttpResultMessage.FilteredResult<RegisterUserResponseWrapper, bool>(userRegistered);
        }

        /// <summary>
        /// Enables user to authenticate using credentials and certificate. 
        /// This authentication is limited, it can be used as one phase of 
        /// MF authentication, because it grants access only to another 
        /// authentication endpoint. OTP code is generated and sent on user's email, 
        /// as well as OTP api key (returned in request).
        /// </summary>
        /// <param name="credentials">User's credentials and certificate</param>
        /// <returns></returns>
        [HttpPost("authenticate/login-form")]
        [SwaggerResponse(StatusCodes.Status200OK, "The results are returned.", typeof(AuthenticateCredentialsResponseWrapper))]
        public async Task<IActionResult> AuthenticateWithCredentials([FromForm] AuthenticateCredentialsPostWrapper credentials)
        {
            CredentialsDTO credentialsDTO = Mapping.Mapper.Map<CredentialsDTO>(credentials);
            ResultMessage<string> result = await userService.AuthenticateWithCredentials(credentialsDTO, credentials.Certificate.AsBasicFileInfo());
            return HttpResultMessage.FilteredResult<AuthenticateCredentialsResponseWrapper, string>(result);
        }

        /// <summary>
        /// User must be authenticated to access this endpoint using specific OTP api key.
        /// This endpoint can be used as second phase of MF authentication.
        /// If authentication with OTP is sucessfull, JWT access token is returned in body, 
        /// and refresh token is returned as cookie. User can use JWT access token limited number 
        /// of minutes, so it is best to refresh token or to login again.
        /// </summary>
        /// <param name="otpInfo">Information regarding OTP code</param>
        /// <param name="otpApiKey">Unique OTP api key</param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = Authentication.OtpApiKeyScheme)]
        [HttpPost("authenticate/otp/{otpApiKey}")]
        [SwaggerResponse(StatusCodes.Status200OK, "The results are returned.", typeof(AuthenticateResponseWrapper))]
        public async Task<IActionResult> AuthenticateWithOTP([FromBody] AuthenticateOtpPostWrapper otpInfo, [FromRoute] string otpApiKey)
        {
            ResultMessage<OutputUserDTO> otpResult = await userService.AuthenticateWithOTP(otpInfo.Otp, otpApiKey);
            if (!otpResult.IsSuccess)
                return HttpResultMessage.FilteredResult(otpResult);

            // Generate access token
            ClaimsIdentity user = jwtManager.CreateSubject(Mapping.Mapper.Map<UserWrapper>(otpResult.Result));
            string jwtToken = jwtManager.GenerateJWTToken(user);
            AuthenticateResponseWrapper authenticateResponseWrapepr = new AuthenticateResponseWrapper()
            {
                AccessToken = jwtToken
            };

            RefreshTokenWrapper refreshToken = jwtManager.GenerateRefreshToken(Request.GetClientIpAddress(HttpContext));
            refreshToken.UserId = otpResult.Result.UserId;

            // Generate refresh token
            RefreshTokenDTO createdRefreshToken = await refreshTokenService.SaveRefreshToken(Mapping.Mapper.Map<RefreshTokenDTO>(refreshToken));
            SetTokenCookie(createdRefreshToken.Token);

            return Ok(authenticateResponseWrapepr);
        }

        /// <summary>
        /// Refreshes token (only once per token). New JWT token is generated, 
        /// as well as new refresh token (Rotation of Refresh Token).
        /// </summary>
        /// <returns></returns>
        [HttpPost("refresh-token")]
        [Authorize(AuthenticationSchemes = Authentication.JwtScheme)]
        [SwaggerResponse(StatusCodes.Status200OK, "The results are returned.", typeof(AuthenticateResponseWrapper))]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                string oldRefreshToken = Request.Cookies[HttpConstants.RefreshToken];
                if (string.IsNullOrEmpty(oldRefreshToken))
                    return BadRequest();

                // Generate access token
                string jwtToken = jwtManager.GenerateJWTToken(User.Identity as ClaimsIdentity);
                AuthenticateResponseWrapper authenticateResponseWrapepr = new AuthenticateResponseWrapper()
                {
                    AccessToken = jwtToken
                };

                RefreshTokenWrapper refreshToken = jwtManager.GenerateRefreshToken(Request.GetClientIpAddress(HttpContext));

                // Generate refresh token
                RefreshTokenDTO createdRefreshToken = await refreshTokenService
                    .UpdateRefreshToken(oldRefreshToken, Mapping.Mapper.Map<RefreshTokenDTO>(refreshToken));

                SetTokenCookie(createdRefreshToken.Token);

                return Ok(authenticateResponseWrapepr);
            }
            catch (MaliciousAttackException)
            {
                await refreshTokenService.RevokeRefreshToken(Request.GetCookie(HttpConstants.RefreshToken), "localhost");
                throw;
            }
        }

        /// <summary>
        /// Revokes current JWT token.
        /// </summary>
        /// <returns></returns>
        [HttpPost("revoke-token")]
        [Authorize(AuthenticationSchemes = Authentication.JwtScheme)]
        [SwaggerResponse(StatusCodes.Status200OK, "The results are returned.", typeof(bool))]
        public async Task<IActionResult> RevokeToken()
        {
            string refreshToken = Request.Cookies[HttpConstants.RefreshToken];

            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest();
            ResultMessage<bool> revoked = await refreshTokenService.RevokeRefreshToken(refreshToken, Request.GetClientIpAddress(HttpContext));
            return HttpResultMessage.FilteredResult(revoked);
        }

        [HttpPost("is-logged-in")]
        [Authorize(AuthenticationSchemes = Authentication.JwtScheme)]
        [SwaggerResponse(StatusCodes.Status200OK, "The results are returned.", typeof(bool))]
        public IActionResult IsLoggedIn()
        {
            return HttpResultMessage.FilteredResult(new ResultMessage<bool>(true));
        }

        private void SetTokenCookie(string token)
        {
            Response.SetCookie(HttpConstants.RefreshToken, token);
        }
    }
}
