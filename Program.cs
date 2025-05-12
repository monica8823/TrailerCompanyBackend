using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TrailerCompanyBackend.Authentication;
using TrailerCompanyBackend.Models;
using TrailerCompanyBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// 加载 JWT 配置到 IOptions
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// 注册数据库上下文
builder.Services.AddDbContext<TrailerCompanyDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 注册服务到 DI 容器
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<StoreService>();
builder.Services.AddScoped<TrailerModelManager>();
builder.Services.AddScoped<ArchiveService>();
builder.Services.AddScoped<AccessoryService>();
builder.Services.AddScoped<MonthService>();
builder.Services.AddScoped<TrailerService>();
builder.Services.AddScoped<ContainerEntryRecordService>();
builder.Services.AddScoped<AccessorySizeService>();
builder.Services.AddScoped<AccessorySizeRelationService>();



// 配置 JWT 身份验证
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

        if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.SecretKey))
        {
            throw new InvalidOperationException("JWT configuration is missing or invalid.");
        }

        var secretKey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(secretKey)
        };
    });

// 添加控制器服务
builder.Services.AddControllers();

// 添加授权服务
builder.Services.AddAuthorization();

// 启用 Swagger 文档生成
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 配置 CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 启用中间件
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
