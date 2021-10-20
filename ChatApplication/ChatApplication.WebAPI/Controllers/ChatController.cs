using System;
using ChatApplication.Core.DTO;
using ChatApplication.Core.ErrorHandling;
using ChatApplication.Core.Services;
using ChatApplication.WebAPI.AutoMapper;
using ChatApplication.WebAPI.ErrorHandling;
using ChatApplication.WebAPI.Models;
using ChatApplication.WebAPI.Security;
using ChatApplication.WebAPI.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace ChatApplication.WebAPI.Controllers
{
    [Route("chat")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Authentication.ApiKeyScheme)]
    public class ChatController : ControllerBase
    {
        private readonly IChatService chatService;
        private readonly IRefreshTokenService refreshTokenService;

        public ChatController(IChatService chatService, IRefreshTokenService refreshTokenService)
        {
            this.chatService = chatService;
            this.refreshTokenService = refreshTokenService;
        }


        /// <summary>
        /// Sends message based on provided body data. It also 
        /// detects malicious attempts to hack system, logs them and 
        /// logs out the user. Only a person with valid JWT token can access this endpoint.
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = Authentication.JwtScheme)]
        [SwaggerResponse(StatusCodes.Status200OK, "The results are returned.", typeof(OutputMessageDTO))]
        public async Task<IActionResult> SendMessage([FromBody] MessagePostWrapper message)
        {
            try
            {
                MessageDTO sendingMessage = Mapping.Mapper.Map<MessageDTO>(message);
                ResultMessage<OutputMessageDTO> resultMessage = await chatService.Send(sendingMessage, (User.Identity as ClaimsIdentity).Name);
                return HttpResultMessage.FilteredResult(resultMessage);
            }
            catch (MaliciousAttackException)
            {
                await refreshTokenService.RevokeRefreshToken(Request.GetCookie(HttpConstants.RefreshToken), "localhost");
                throw;
            }
        }

        /// <summary>
        /// Reads all messages that belong to certain user.
        /// User must be logged in and can access only his messages.
        /// </summary>
        /// <param name="username">Username for which messages are requested</param>
        /// <returns></returns>
        [HttpGet("{username}")]
        [Authorize(AuthenticationSchemes = Authentication.JwtScheme)]
        [SwaggerResponse(StatusCodes.Status200OK, "The results are returned.", typeof(List<ChatDTO>))]
        public async Task<IActionResult> ReadAll([FromRoute] string username)
        {
            try
            {
                List<ChatDTO> chat = await chatService.ReadChat(username, (User.Identity as ClaimsIdentity)?.Name);
                return Ok(chat);
            }
            catch (ForbiddenAccessException)
            {
                await refreshTokenService.RevokeRefreshToken(Request.GetCookie(HttpConstants.RefreshToken), "localhost");
                throw;
            }
        }
    }
}
