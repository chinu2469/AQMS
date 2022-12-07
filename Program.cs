using AQMS.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;    // this package for logging
//using Microsoft.Extensions.Logging;



var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<AQMSapiDbContext>(Options => Options.UseInMemoryDatabase("AQMSdata"));
builder.Services.AddDbContext<AQMSapiDbContext>(Options => Options.UseSqlServer(
    builder.Configuration.GetConnectionString("AQMSapiConString")
    ));

//logs
var loggrer = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("log/AQMSlogs.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();                     // serilog obj


builder.Logging.ClearProviders();       // clean old log method
builder.Logging.AddSerilog(loggrer);   //  adding serilog as log



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
