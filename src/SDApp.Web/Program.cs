using SDApp.Web;
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

var postgresConnectionString = builder.Configuration.GetConnectionString(AppDbContext.ConnectionName) ??
    throw new InvalidOperationException($"Connection string '{AppDbContext.ConnectionName}' not found.");

builder.Services
    .AddDbContext<AppDbContext>(
        options => AppDbContext.Configure(options, postgresConnectionString),
        contextLifetime: ServiceLifetime.Scoped,
        optionsLifetime: ServiceLifetime.Singleton)
    .AddDbContextFactory<AppDbContext>(
        options => AppDbContext.Configure(options, postgresConnectionString),
        lifetime: ServiceLifetime.Singleton);

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

app.MapGet("/health", () => "OK");

app.MapGet("/", () => "Hello World!");

app.Run();
