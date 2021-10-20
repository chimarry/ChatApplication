namespace ChatApplication.Core.Options
{
    public class AuthenticationOptions
    {
        public int AccessTokenLifetimeMin { get; set; }
        public int RefreshTokenLifetimeDay { get; set; }
        public string Secret { get; set; }
    }
}
