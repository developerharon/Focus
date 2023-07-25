using Toolbelt.Blazor;

namespace Focus.UI.Services
{
    public class HttpInterceptorService
    {
        private readonly HttpClientInterceptor _interceptor;
        private readonly RefreshTokenService _refreshTokenService;

        public HttpInterceptorService(HttpClientInterceptor interceptor, RefreshTokenService refreshTokenService)
        {
            _interceptor = interceptor;
            _refreshTokenService = refreshTokenService;
        }

        public void RegisterEvent() => _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;

        private async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
        {
            var absPath = e.Request?.RequestUri?.AbsolutePath;

            try
            {
                if (absPath != null && !absPath.Contains("user"))
                {
                    var accessToken = await _refreshTokenService.TryRefreshTokenAsync();

                    if (e.Request != null && !string.IsNullOrEmpty(accessToken))
                        e.Request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", accessToken);
                }
            }
            catch (Exception)
            {
                if (e.Request is not null)
                    e.Request.Headers.Authorization = null;
            }
        }

        public void DisposeEvent() => _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
    }
}