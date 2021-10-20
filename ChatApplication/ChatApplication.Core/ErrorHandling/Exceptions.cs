using ChatApplication.Core.Entities;
using System;

namespace ChatApplication.Core.ErrorHandling
{
    /// <summary>
    /// This exception should be thrown when unauthenticated request for application resource was identified.
    /// </summary>
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("Unauthorized.") { }
    }

    /// <summary>
    /// This exception should be thrown when authenticated user tried to access resource with no privilege to do so.
    /// </summary>
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException() : base("Forbidden access to the resource.") { }
    }

    /// <summary>
    /// This exception should be thrown when invalid API key was provided in request.
    /// </summary>
    public class ApiKeyAuthenticationException : Exception
    {
        public ApiKeyAuthenticationException() : base("Invalid API key.") { }
    }

    /// <summary>
    /// This exception should be thrown when there is some suspicous acts related to refresh token.
    /// </summary>
    public class RefreshTokenException : Exception
    {
        public RefreshTokenException() : base(MaliciousAttack.ManInTheMiddle.GetMessage()) { }
    }

    public class MaliciousAttackException : Exception
    {
        public MaliciousAttackException() : base("Malicious attack was detected.") { }

        public MaliciousAttackException(MaliciousAttack attack) : base(attack.GetMessage()) { }
    }
}
