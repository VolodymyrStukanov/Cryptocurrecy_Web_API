
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApplication1.DB;
using WebApplication1.Services;
using WebApplication1.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(option =>
{
    option.ClearProviders();
    var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/error/log-.txt", rollingInterval: RollingInterval.Day/*, restrictedToMinimumLevel: LogEventLevel.Warning*/)
            .CreateLogger();
    option.AddSerilog(logger, dispose: true);
});

builder.Services.AddDbContext<ApplicationDbContext>((options) =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddTransient<ICurrencyService, CurrencyService>();

builder.Services.AddHostedService<RealTimeUpdateService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
