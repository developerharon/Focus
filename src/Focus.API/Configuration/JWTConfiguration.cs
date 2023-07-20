namespace Focus.API.Configuration
{
    public class JWTConfiguration
    {
        public string? Secret { get; set; }
        public string? ValidIssuer { get; set; }
        public string? ValidAudience { get; set; }
        public int? DurationInMinutes { get; set; }
        public int? RefreshTokenDurationInDays { get; set; }
    }
}