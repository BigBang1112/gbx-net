using GbxExplorerOld.Client;
using GbxExplorerOld.Client.Sections;
using GbxExplorerOld.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Globalization;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

GBX.NET.Gbx.LZO = new GBX.NET.LZO.Lzo();
GBX.NET.Gbx.CRC32 = new GBX.NET.Hashing.CRC32();
GBX.NET.Gbx.ZLib = new GBX.NET.ZLib.ZLib();

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

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