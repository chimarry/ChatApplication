# Chat application
------------------------

A system that enables chat between multiple persons. Multifactor authentication is used. Steps are:
- As a result of the registration process, a user gets the certificate.
- First phase of authentication is based on username, password and certificate.
- If the certificate and credentials are valid, an OTP code is generated and sent to the user's email. OTP is valid for a couple of minutes (short-lived).
- As the second phase of authentication, the user enters previously given OTP code.
- If OTP is invalid, a man in the middle attack is registered and login fails. If OTP is valid, the JWT token is generated and returned to the user.
- After successful login, the user can see a list of other users, and start chatting. The user also sees the history of messages. 

### Requirements
-------------------
The system manages the process of authentication, session, detects malicious messages and logs out the user if such message is received and to creates and validates users' certificates.
Communication is encrypted (HTTPS). It detects malicious attacks such as SQL Injection, XSS, Man in The Middle, Paramterer tampering and prevents DoS attacks at some level (IP rate limiting). If such an attack is detected, information about the attack is saved in the database, the user is automatically logged out and notified about the attack (to scare the attacker).

### Technologies and libraries
---------------------------------
- ASP.NET Core 3.1 (Web API)
- AutoMapper
- MySQL
- Microsoft Dependency Injection
- Microsoft.AspNetCore.Authentication.JwtBearer
- Angular and Angular Material UI

### Authentication process using JWT and .NET
---------------------------------
Because the JWT token contains sensitive data, it is short-lived, and the user must provide a new token for authentication purposes each time the old token expires. It would not be appropriate to ask the user to repeat previous MF steps to log in, so in this project, a concept with a refresh token is used. When JWT token is generated, a refresh token is created as well and returned to the user as HttpOnly and Secure cookie. The refresh token is valid for a longer period. [Refresh Token Rotation](https://auth0.com/docs/security/tokens/refresh-tokens/refresh-token-rotation) is implemented, so that each time a new JWT token is created, the old refresh token is replaced with a new one (such activity is saved in the database), so if someone tries to log in with old refresh token, the man in the middle attack is detected. There is an option to revoke the token, but JWT has the drawback that the session cannot be easily terminated (it is valid as long as it has not expired).
ASP.NET provides attributes, middlewares and dependency injection for purpose of authentication and authorization.
~~~~~~
 services.AddAuthentication()
         .AddScheme<JwtAuthenticationOptions,JwtAuthenticationHandler>
                   (Authentication.JwtScheme, null)
         .AddScheme<ApiKeyAuthenticationOptions,ApiKeyAuthenticationHandler>
                   (Authentication.ApiKeyScheme, null)
         .AddScheme<OtpAuthenticationOptions, OtpAuthenticationHandler>
                   (Authentication.OtpApiKeyScheme, null);
~~~~~~

Application is secured with an API key (UUID hard to guess), so each controller is protected with an authorization attribute that checks if the HTTP request contains a valid API key.
~~~~~~
    /// <summary>
    /// Enpoints for working with users
    /// </summary>
    [Route("users")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Authentication.ApiKeyScheme)]
    public class UserController : ControllerBase
    { ... }
~~~~~~
Also, because the OTP code is sent to the user via email, an endpoint that receives this OTP code has one more layer of protection - OTP API key, which was returned to the user when the OTP code was requested. 
~~~~~
        /// <summary>
        /// User must be authenticated to access this endpoint using specific OTP api key.
        /// This endpoint can be used as second phase of MF authentication.
        /// If authentication with OTP is sucessfull, JWT access token is returned in body, 
        /// and refresh token is returned as cookie. User can use JWT access token 
        /// limited number of minutes, so it is best to refresh token or to login again.
        /// </summary>
        /// <param name="otpInfo">Information regarding OTP code</param>
        /// <param name="otpApiKey">Unique OTP api key</param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = Authentication.OtpApiKeyScheme)]
        [HttpPost("authenticate/otp/{otpApiKey}")]
        [SwaggerResponse(StatusCodes.Status200OK, "The results are returned.", typeof(AuthenticateResponseWrapper))]
        public async Task<IActionResult> AuthenticateWithOTP
           ([FromBody] AuthenticateOtpPostWrapper otpInfo, [FromRoute] string otpApiKey)
        { ... }
~~~~~
Other endpoints are protected with the JWT layer of protection - a new authorization attribute set on this endpoint.
~~~~~~~~
        /// <summary>
        /// Sends message based on provided body data. It also 
        /// detects malicious attempts to hack system, logs them and 
        /// logs out the user. Only a person with valid JWT token can access this endpoint.
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = Authentication.JwtScheme)]
        public async Task<IActionResult> SendMessage([FromBody] MessagePostWrapper message)
        { ... }  
~~~~~~~~~~~

As already mentioned, authorization attributes are used, as well as authorization/authentication handlers. 

### Angular
---------------
Material UI is used to design front-end application. Guard is used to protect application URLs. Certificate is set in order to use HTTPS communication with server. 
Design is shown on images below.
First phase of authentication:  
![1](/0.jpg)  

Second phase of authentication:  
![2](/1.jpg)  

Chat screen:  
![3](/2.jpg)