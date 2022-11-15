global using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Cliente.Client;
using Cliente.Client.Auth;
using Cliente.Client.Store;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>(provider => provider.GetRequiredService<AuthStateProvider>());
builder.Services.AddScoped<IAuth, AuthStateProvider>(provider => provider.GetRequiredService<AuthStateProvider>());
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AppState>();

await builder.Build().RunAsync();
