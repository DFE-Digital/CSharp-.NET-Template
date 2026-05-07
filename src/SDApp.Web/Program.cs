using GovUk.Frontend.AspNetCore;
using Serilog;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);

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
});

builder.Services.AddMvc();

builder.Services.AddGovUkFrontend();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSassCompiler();
}

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

app.UseStatusCodePagesWithReExecute("/error", "?code={0}");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseGovUkFrontend();

app.MapStaticAssets();

app.MapControllers()
    .WithStaticAssets();

app.MapGet("/health", () => "OK");

app.Run();
