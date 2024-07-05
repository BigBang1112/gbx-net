using Discord.Interactions;
using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GbxDiscordBot;
using GBX.NET.Extensions;
using GBX.NET.LZO;
using GBX.NET.Hashing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GbxDiscordBot.Services;
using OpenTelemetry.Metrics;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) =>
{
    services.AddSingleton(TimeProvider.System);

    services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlite(context.Configuration.GetConnectionString("DefaultConnection"));
    });

    // Configure Discord bot
    services.AddSingleton(new DiscordSocketConfig()
    {
        LogLevel = LogSeverity.Verbose
    });

    // Add Discord bot client and Interaction Framework
    services.AddSingleton<DiscordSocketClient>();
    services.AddSingleton<InteractionService>(provider => new(provider.GetRequiredService<DiscordSocketClient>(), new()
    {
        LogLevel = LogSeverity.Verbose,
        //LocalizationManager = new JsonLocalizationManager("Localization", "commands")
    }));

    services.AddLogging(builder =>
    {
        builder.AddOpenTelemetry(options =>
        {
            options.IncludeScopes = true;
            options.IncludeFormattedMessage = true;
            options.AddOtlpExporter();
        });
    });

    // Add startup
    services.AddHostedService<Startup>();

    // Add services
    services.AddHttpClient();
    services.AddSingleton<IDiscordBot, DiscordBot>();
    services.AddSingleton<ILzo, Lzo>();
    services.AddSingleton<ICrc32, CRC32>();

    services.AddScoped<IGbxService, GbxService>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IResponseService, ResponseService>();

    services.AddMemoryCache();

    services.AddOpenTelemetry()
        .WithMetrics(options =>
        {
            options
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddProcessInstrumentation()
                .AddOtlpExporter();
            
            options.AddMeter("System.Net.Http");
        })
        .WithTracing(options =>
        {
            if (context.HostingEnvironment.IsDevelopment())
            {
                options.SetSampler<AlwaysOnSampler>();
            }

            options
                .AddHttpClientInstrumentation()
                .AddEntityFrameworkCoreInstrumentation()
                .AddOtlpExporter();
        });
    services.AddMetrics();
});

await builder.Build().RunAsync();