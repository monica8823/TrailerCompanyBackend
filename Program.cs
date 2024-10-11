using Microsoft.EntityFrameworkCore;
using TrailerCompanyBackend.Models;
using TrailerCompanyBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TrailerCompanyDbContext>(options =>
    options.UseSqlite("Data Source=../instance/trailer_company.db"));

// Add StoreService to the DI container.
builder.Services.AddScoped<StoreService>();

// Add services to the container.
builder.Services.AddControllers();

// Enable API documentation (Swagger)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Map controllers to handle API requests.
app.MapControllers();

app.Run();
