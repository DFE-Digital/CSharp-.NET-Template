using GovUk.Frontend.AspNetCore;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.DataProtection;
using Prometheus;
using SDApp.Web;
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
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddMvc();

builder.Services.AddDataProtection()
    .PersistKeysToDbContext<AppDbContext>();

builder.Services.AddHealthChecks()
    .AddNpgSql(postgresConnectionString);

builder.Services.AddGovUkFrontend();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSassCompiler();
}

builder.Services.AddDistributedPostgresCache(options =>
{
    options.ConnectionString = postgresConnectionString;
    options.CreateIfNotExists = true;
});

builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(options => options.UseNpgsqlConnection(postgresConnectionString)));

builder.Services.AddHangfireServer();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/error");
}

app.UseStatusCodePagesWithReExecute("/error", "?code={0}");

app.UseRouting();

app.UseHttpMetrics();

app.UseAuthentication();
app.UseAuthorization();

app.UseGovUkFrontend();

app.MapStaticAssets();

app.MapControllers()
    .WithStaticAssets();

app.MapGet("/health", () => "OK");

app.MapMetrics("/metrics");

// The Hangfire dashboard is only enabled in Development, where it is restricted to local requests.
// An authorization policy MUST be specified before enabling the dashboard in any other environment,
// otherwise anyone could view and manage background jobs. Replace the call below with:
//
//     app.MapHangfireDashboardWithAuthorizationPolicy(authorizationPolicyName: "YourPolicyName", "/hangfire");
//
// where "YourPolicyName" is a policy registered with builder.Services.AddAuthorization(...).
if (app.Environment.IsDevelopment())
{
    app.MapHangfireDashboard("/hangfire");
}

app.UseHealthChecks("/status");

app.Run();
