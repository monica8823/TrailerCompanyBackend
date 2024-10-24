using Microsoft.EntityFrameworkCore;
using TrailerCompanyBackend.Models;
using TrailerCompanyBackend.Services;


var builder = WebApplication.CreateBuilder(args);

// Register the database context with a connection string from appsettings.json
builder.Services.AddDbContext<TrailerCompanyDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services in the DI container
builder.Services.AddScoped<StoreService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TrailerService>();
builder.Services.AddScoped<TrailerModelManager>();
builder.Services.AddScoped<ArchiveService>();


// Add controllers to the DI container
builder.Services.AddControllers();

// Enable Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

// Map controllers to handle API requests
app.MapControllers();

app.Run();
