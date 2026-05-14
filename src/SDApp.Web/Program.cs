using Serilog;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseSentry();

builder.Services.AddSerilog((services, config) =>
{
    config.ReadFrom.Configuration(builder.Configuration);

    if (builder.Environment.IsDevelopment())
    {
        config.WriteTo.Console(formatProvider: System.Globalization.CultureInfo.CurrentCulture);
    }
    else
    {
        config.WriteTo.Console(new CompactJsonFormatter());
    }
    
    config.WriteTo.Sentry(options =>
    {
        options.InitializeSdk = false;  // SDK is initialized by builder.WebHost.UseSentry();
        options.MinimumBreadcrumbLevel = Serilog.Events.LogEventLevel.Debug;
        options.MinimumEventLevel = Serilog.Events.LogEventLevel.Error;
    });
});

var app = builder.Build();

app.UseSerilogRequestLogging();

app.MapGet("/health", () => "OK");

app.MapGet("/", () => "Hello World!");

app.Run();
