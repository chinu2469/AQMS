using AQMS.Data;
using AQMS.Data.repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;    // this package for logging
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.PortableExecutable;
using System.Text;
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

//jwt token 
builder.Services.AddAuthorization();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DemoJWT", Version = "v1" });
//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Description = "Jwt Authorization",
//        Name = "Authorization",
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.ApiKey,
//        Scheme = "Bearer"
//    });

//    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
//    {
//    new OpenApiSecurityScheme
//    {
//        Reference = new OpenApiReference
//        {
//            Type = ReferenceType. SecurityScheme,
//            Id= "Bearer"
//        }
//        },
//        new string[] {}
//        }
//    });

//});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:key"]))
    };
});

//logs
var loggrer = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("log/AQMSlogs.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();                     // serilog obj


builder.Logging.ClearProviders();       // clean old log method
builder.Logging.AddSerilog(loggrer);   //  adding serilog as log

builder.Services.AddTransient<IAqmsRepository,AqmsRepository>();

//configure for localhost
var devCorsPolicy = "devCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(devCorsPolicy, builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

//http setup


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DemoJWTToken v1"));
    app.UseCors(devCorsPolicy);
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
