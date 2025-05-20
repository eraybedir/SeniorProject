using MarketAPI.Data;
using MarketAPI.Interfaces;
using MarketAPI.Repositories;
using MarketAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

//CORS(Cross-Origin Resource Sharing) CORS, farkl domainler (alan adlar) arasnda web isteklerine izin verilip verilmemesini kontrol eder. Flutter<->.NET API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the URLs
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://0.0.0.0:{port}");

// Only use HTTPS redirection in production and when not running on Render
if (app.Environment.IsProduction() && !app.Environment.IsEnvironment("Render"))
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.Run();
