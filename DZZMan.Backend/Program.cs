using DZZMan.Backend;
using DZZMan.Backend.Database;
using DZZMan.Backend.Database.Providers;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] [{SourceContext}] {Message}{NewLine}{Exception}",
                     theme: SystemConsoleTheme.Colored)
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration.GetSection("Database").Get<Settings>();

builder.Logging.AddSerilog(dispose: true);

builder.Services
    .AddSingleton<Settings>(settings)
    .AddSingleton<DocumentStoreProvider>()
    .AddSingleton<SatelliteProvider>()
    .AddSingleton<TokenProvider>();

// Add services to the container.
builder.Services
    .AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseStatusCodePages();
app.MapControllers();

app.Run();
