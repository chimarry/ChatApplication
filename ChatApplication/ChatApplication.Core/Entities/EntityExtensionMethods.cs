using System;

namespace ChatApplication.Core.Entities
{
    public static class EntityExtensionMethods
    {
        public static bool IsActive(this RefreshToken token)
             => token.RevokedOn == null && !token.IsExpired();

        public static bool IsExpired(this RefreshToken token)
             => DateTime.UtcNow >= DateTime.SpecifyKind(token.ExpiresOn, DateTimeKind.Utc);
    }
}
