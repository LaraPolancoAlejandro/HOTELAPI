using HOTELAPI1;
using HOTELAPI1.Services;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using HOTELAPI1.Abstract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
//builder.Services.AddTransient<EmailService>(); // Comentado ya que parece que no lo necesitas ahora
builder.Services.AddTransient<ComentarioService>();
builder.Services.AddTransient<IPropiedadService, PropiedadService>();
builder.Services.AddTransient<ClienteService>();
builder.Services.AddTransient<ReservacionService>();

// Register HotelDbContext
builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register MongoDB Client
builder.Services.AddSingleton<IMongoClient, MongoClient>(
    _ => new MongoClient(builder.Configuration["MongoDB:ConnectionString"])
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

app.MapControllers();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://*:{port}/");
