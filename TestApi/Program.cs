using System.Net;
using Amazon;
using Amazon.S3;
using DockerWebAPI.DbStuff;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using HealthChecks.Aws.S3;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TestApi.Io;
using TestApi.Io.ExternalService;
using TestApi.Services;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
    .Enrich.FromLogContext().Enrich.WithProperty("appName", "sampleapp")
    .CreateLogger();

builder.Logging.AddSerilog();
//
// builder.Services.AddDbContext<RandomDbContext>(options =>
// {
//     var connectionString = @"Server=localhost;Database=bob;Uid=root;Pwd=root;";
//     options.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString));
// });
builder.Services.AddControllers().AddControllersAsServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBusinessLogicImplementation,BusinessLogicImplementation>();
builder.Services.AddScoped<IDbRepoImplementation,DbRepoImplementation>();
builder.Services.AddScoped<IThirdPartyService,ThirdPartyService>();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();
app.UseRouting();
app.Run();