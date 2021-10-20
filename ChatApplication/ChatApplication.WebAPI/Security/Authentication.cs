namespace ChatApplication.WebAPI.Security
{
    public static class Authentication
    {
        public const string ApiKeyScheme = "ApiKeyScheme";
        public const string OtpApiKeyScheme = "OtpApiKeyScheme";
        public const string JwtScheme = "JwtScheme";

        public const string ApiKeyAndOptKeyScheme = "ApiKeyScheme,OtpApiKeyScheme";
        public const string ApiKeyAndJwtScheme = "ApiKeyScheme,JwtScheme";
    }
}
