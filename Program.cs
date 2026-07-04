using System.Text;
using DeanInfoSystem.Application.Common.Auth;
using DeanInfoSystem.Application.Common.Caching;
using DeanInfoSystem.Application.Users;
using DeanInfoSystem.Infrastructure.Caching;
using DeanInfoSystem.Infrastructure.Repos;
using FluentValidation;
using FluentValidation.AspNetCore;
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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

//DB connect (postgres db)
builder.Services.AddDbContext<SystemDbContext>(options =>
    options.UseNpgsql(builder.Configuration["ConnectionStrings:PostgreSQL"]));


//Cache connect (redis cache)

var multiplexer = ConnectionMultiplexer.Connect(builder.Configuration["ConnectionStrings:RedisConnection"]!);
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);
builder.Services.AddScoped<ICacheService, RedisCache>();

//Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();


//Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();


//Controllers
builder.Services.AddControllers();


//Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

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
                                    builder.Configuration["Security:SecretKey"]!)),
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
                                    .GetRequiredService<ICacheService>()
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
    .AddPolicy("Student", policy => policy.RequireRole("Student"))
    .AddPolicy("DeanViceDeanSecretary", policy =>
    {
        policy.RequireAssertion(ctx =>
        {
            return ctx.User.IsInRole("Dean") |
                    ctx.User.IsInRole("ViceDean") |
                    ctx.User.IsInRole("Secretary");
        });
    });

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
//API Docs
app.MapOpenApi();
app.MapScalarApiReference();


app.UseAuthentication();
app.UseAuthorization();
//App init
using (var scope = app.Services.CreateScope())
{
    var AuthService = scope.ServiceProvider.GetRequiredService<IAuthService>();
    var AdminCreds = await AuthService.GetAdministratorAsync();
    if (AdminCreds is null)
    {
        Console.WriteLine("\n\n\nAdmin has been instantiated. Take a peek at the db.\n\n\n");
    }
    else
    {
        Console.WriteLine($"\n\n\nCredentials:\nUsername: {AdminCreds[0]}\nPassword: {AdminCreds[1]}\nDon't forget those.\n\n\n");
    }

}

app.MapControllers();

app.Run();
