namespace Focus.UI.Infrastructure
{
    public static class ServerUrls
    {
        private static string apiVersion => "v1.0";
        public static string BaseUrl => "https://localhost:7172";
        public static string LoginUrl = $"/api/{apiVersion}/users/login";
        public static string LogoutUrl = "";
        public static string RefreshTokenUrl = "";
    }
}