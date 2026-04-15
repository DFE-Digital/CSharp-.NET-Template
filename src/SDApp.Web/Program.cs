var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/health", () => "OK");

app.MapGet("/", () => "Hello World!");

app.Run();
