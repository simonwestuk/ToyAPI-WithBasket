namespace ToyAPI.Classes
{
    public class JwtBearerTokenSettings
    {
        //config class to set up Token Settings
        public string SecretKey { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int ExpiryTimeInSeconds { get; set; }
    }
}
