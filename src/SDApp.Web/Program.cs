using Serilog;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog((services, config) =>
{
    config.ReadFrom.Configuration(builder.Configuration);

    if (builder.Environment.IsDevelopment())
    {
        config.WriteTo.Console();
    }
    else
    {
        config.WriteTo.Console(new CompactJsonFormatter());
    }
});

var app = builder.Build();

app.UseSerilogRequestLogging();

app.MapGet("/", () => "Hello World!");

app.Run();
