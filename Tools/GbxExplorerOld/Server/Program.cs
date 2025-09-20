using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.FileProviders;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Reflection;
using System.Runtime.Versioning;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console(theme: AnsiConsoleTheme.Sixteen, applyThemeToRedirectedOutput: true)
    .WriteTo.OpenTelemetry(options =>
    {
        options.Endpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
        options.Protocol = builder.Configuration["OTEL_EXPORTER_OTLP_PROTOCOL"]?.ToLowerInvariant() switch
        {
            "grpc" => Serilog.Sinks.OpenTelemetry.OtlpProtocol.Grpc,
            "http/protobuf" or null or "" => Serilog.Sinks.OpenTelemetry.OtlpProtocol.HttpProtobuf,
            _ => throw new NotSupportedException($"OTLP protocol {builder.Configuration["OTEL_EXPORTER_OTLP_PROTOCOL"]} is not supported")
        };
        options.Headers = builder.Configuration["OTEL_EXPORTER_OTLP_HEADERS"]?
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Split('=', 2, StringSplitOptions.RemoveEmptyEntries))
            .ToDictionary(x => x[0], x => x[1]) ?? [];
    })
    .CreateLogger();

builder.Services.AddSerilog();

builder.Services.AddOpenTelemetry()
    .WithMetrics(options =>
    {
        options
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation()
            .AddOtlpExporter();

        options.AddMeter("System.Net.Http");
    })
    .WithTracing(options =>
    {
        if (builder.Environment.IsDevelopment())
        {
            options.SetSampler<AlwaysOnSampler>();
        }

        options
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter();
    });
builder.Services.AddMetrics();

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
builder.Services.AddRazorPages();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var assembly = Assembly.GetExecutingAssembly();
    var attr = assembly.GetCustomAttribute<TargetFrameworkAttribute>();
    var version =  attr!.FrameworkName.Split('=')[1];
    var netVersion = "net" + version.TrimStart('v');
    var dllPath = Path.Combine(builder.Environment.ContentRootPath, "..", "Client", "bin", "Debug", netVersion);

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(dllPath),
        RequestPath = "/refs",
        ServeUnknownFileTypes = true,
    });
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true,
    DefaultContentType = "application/octet-stream"
});

app.UseRouting();

app.UseResponseCompression();

app.UseForwardedHeaders(new()
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("{**slug}", "index.html");

app.Run();