using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyGbxBlazorWasmApp;
using GBX.NET;

#if (EnableLzo)
Gbx.LZO = new GBX.NET.LZO.Lzo();
#endif
#if (EnableZlib)
Gbx.ZLib = new GBX.NET.ZLib.ZLib();
#endif
#if (EnableCrc32)
Gbx.CRC32 = new GBX.NET.Hashing.CRC32();
#endif

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
