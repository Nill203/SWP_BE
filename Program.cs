using Microsoft.EntityFrameworkCore;
using BloodDonationBE.Data;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using BloodDonationBE.Features.Users;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using BloodDonationBE.Services;
using BloodDonationBE.Features.BloodDonationCampaigns;
using BloodDonationBE.Features.CampaignRegistrations;
using BloodDonationBE.Features.BloodUnits;
using BloodDonationBE.Features.Hospitals;
using BloodDonationBE.Features.BloodRequests; // <-- QUAN TRỌNG: Thêm using cho module mới

var builder = WebApplication.CreateBuilder(args);

// --- 1. Cấu hình Database Context (MySQL) ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// --- 2. Đăng ký các dịch vụ tùy chỉnh ---
builder.Services.AddUserServices();
builder.Services.AddBloodDonationCampaignServices();
builder.Services.AddCampaignRegistrationServices();
builder.Services.AddBloodUnitServices();
builder.Services.AddHospitalServices();
builder.Services.AddBloodRequestServices();

// --- 3. Cấu hình xác thực JWT (JSON Web Token) ---
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("Chưa cấu hình 'Jwt:Key' trong appsettings.json");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// Thêm các dịch vụ vào container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Chuyển đổi Enums thành chuỗi trong các phản hồi API để dễ đọc hơn
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// --- 4. Cấu hình Swagger để hỗ trợ JWT ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "BloodDonationBE API", Version = "v1" });

    // Định nghĩa scheme "Bearer" cho việc xác thực
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Ví dụ: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Yêu cầu Swagger sử dụng scheme xác thực đã định nghĩa
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// --- 5. Thêm chính sách CORS ---
// Cho phép frontend của bạn gọi đến API này
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();

// Cấu hình pipeline xử lý HTTP request.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// --- 6. Sử dụng CORS ---
app.UseCors("AllowAll");

// --- 7. Sử dụng Authentication và Authorization ---
// UseAuthentication phải được gọi trước UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
