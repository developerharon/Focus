using Blazored.LocalStorage;
using Focus.UI;
using Focus.UI.Infrastructure;
using Focus.UI.Services;
using Focus.UI.ViewModels.Users;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(ServerUrls.BaseUrl) }.EnableIntercept(sp));
builder.Services.AddHttpClientInterceptor();
builder.Services.AddMudServices();

builder.Services.AddTransient<LoginUserViewModel>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddTransient<RefreshTokenService>();
builder.Services.AddTransient<LogoutService>();
builder.Services.AddTransient<HttpInterceptorService>();

await builder.Build().RunAsync();
