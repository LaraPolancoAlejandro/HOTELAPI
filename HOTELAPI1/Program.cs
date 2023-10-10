using HOTELAPI1;
using HOTELAPI1.Services;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using HOTELAPI1.Abstract;
using HOTELAPI1.Models;
using Amazon.Util.Internal.PlatformServices;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHostedService<BackGroundWorker>();
builder.Services.AddTransient<ComentarioService>();
builder.Services.AddTransient<IPropiedadService, PropiedadService>();
builder.Services.AddTransient<ClienteService>();
builder.Services.AddTransient<ReservacionService>();
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));

// Register HotelDbContext
var defaultConnection = Environment.GetEnvironmentVariable("DEFAULT_CONNECTION_STRING");
builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseSqlServer(defaultConnection));

// Register MongoDB Client
var mongoConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");
builder.Services.AddSingleton<IMongoClient, MongoClient>(
    _ => new MongoClient(mongoConnectionString)
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var options = new AppSettings();
app.Configuration.GetSection("ApplicationSettings").Bind(options);

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

app.MapControllers();

//var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://*:{options.Port}/");