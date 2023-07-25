namespace Focus.UI.Extensions
{
    public static class HttpClientExtensions
    {
        public static void SetBearerAuthenticationHeader(this HttpClient httpClient, string accessToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", accessToken);
        }

        public static void RemoveBearerAuthenticationHeader(this HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}