using System.Text;
using DeanInfoSystem.Application.Extensions;
using DeanInfoSystem.Application.Extensions.Implementation;
using DeanInfoSystem.Infrastructure.Repos;
using DeanInfoSystem.Infrastructure.Repos.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using StackExchange.Redis;

//Application configurator
var builder = WebApplication.CreateBuilder(args);

//Allow angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins(["http://localhost:4200", "http://192.168.100.5:4200", "http://0.0.0.0:4200"]) // Your Angular URL
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

//DB connect (postgres db)
builder.Services.AddDbContext<SystemDbContext>(options =>
    options.UseNpgsql(builder.Configuration["ConnectionStrings:PostgreSQL"]));


//Cache connect (redis cache)

var multiplexer = ConnectionMultiplexer.Connect(builder.Configuration["ConnectionStrings:RedisConnection"]!);
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);
builder.Services.AddScoped<IRedisCacheExt, RedisCacheExt>();

//Services



//Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();


//Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
        conf =>
        {
            conf.RequireHttpsMetadata = false;
            conf.Audience = builder.Configuration["API:Audience"];
            conf.SaveToken = true;
            conf.TokenValidationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                                    builder.Configuration["Secrets:SecretKey"]!)),
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["API:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["API:Audience"],
                ClockSkew = TimeSpan.FromMinutes(5)
            };
            conf.Events = new JwtBearerEvents
            {
                OnMessageReceived = async context =>
                {
                    context.Token = context.Request.Cookies["AccessToken"];
                },
                OnTokenValidated = async context =>
                {
                    var TokenToCheck = context
                                            .HttpContext
                                            .Request
                                            .Cookies["AccessToken"];
                    var _redis = context
                                    .HttpContext
                                    .RequestServices
                                    .GetRequiredService<IRedisCacheExt>()
                                    .GetRedis();
                    var isRevoked = await _redis.StringGetAsync($"Revoked_{TokenToCheck}");
                    if (!string.IsNullOrEmpty(isRevoked))
                    {
                        context.Fail("Token has been revoked.");
                    }
                    context.Success();
                }
            };
        }
    );


builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Dean", policy => policy.RequireRole("Dean"))
    .AddPolicy("Secretary", policy => policy.RequireRole("Secretary"))
    .AddPolicy("ViceDean", policy => policy.RequireRole("ViceDean"))
    .AddPolicy("Assisstant", policy => policy.RequireRole("Assisstant"))
    .AddPolicy("EducationalAdvisor", policy => policy.RequireRole("EducationalAdvisor"))
    .AddPolicy("Student", policy => policy.RequireRole("Student"));

var app = builder.Build();


//API Docs
app.MapScalarApiReference();


//App init
using (var scope = app.Services.CreateScope())
{

}

app.Run();
