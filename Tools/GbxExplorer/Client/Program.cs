using GbxExplorer.Client;
using GbxExplorer.Client.Sections;
using GbxExplorer.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Globalization;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Toolbelt.Blazor.Extensions.DependencyInjection;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

GBX.NET.Lzo.SetLzo(typeof(GBX.NET.LZO.MiniLZO));

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddPWAUpdater();

builder.Services.AddScoped<AuthenticationStateProvider, DiscordAuthenticationStateProvider>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IGbxService, GbxService>();
builder.Services.AddScoped<IDownloadStreamService, DownloadStreamService>();
builder.Services.AddSingleton<ISettingsService, SettingsService>();
builder.Services.AddSingleton<ILogger, MemoryLogger>();
builder.Services.AddSingleton<IFaultyGbxService, FaultyGbxService>();
builder.Services.AddSingleton<IValueRendererService, ValueRendererService>();
builder.Services.AddSingleton<IValuePreviewService, ValuePreviewService>();
builder.Services.AddSingleton<ITypeCacheService, TypeCacheService>();
builder.Services.AddSingleton<ISelectionService, SelectionService>();
builder.Services.AddSingleton<IOpenChunkService, OpenChunkService>();
builder.Services.AddSingleton<IBaseAddressService, BaseAddressService>();
builder.Services.AddSingleton<MemoryLog>();

await builder.Build().RunAsync();