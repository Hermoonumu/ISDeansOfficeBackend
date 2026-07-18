using System.Text;
using DeanInfoSystem.Application.Analytics;
using DeanInfoSystem.Application.Common.Auth;
using DeanInfoSystem.Application.Common.Caching;
using DeanInfoSystem.Application.Common.Handlers;
using DeanInfoSystem.Application.Common.UoW;
using DeanInfoSystem.Application.Curricula;
using DeanInfoSystem.Application.Enrollment;
using DeanInfoSystem.Application.Programs;
using DeanInfoSystem.Application.StudentGrades;
using DeanInfoSystem.Application.Subjects;
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

//Exception Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

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
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


//Cache connect (redis cache)

var multiplexer = ConnectionMultiplexer.Connect(builder.Configuration["ConnectionStrings:RedisConnection"]!);
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);
builder.Services.AddScoped<ICacheService, RedisCache>();

//Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IProgramService, ProgramService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IStudentGradeService, StudentGradeService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();



//Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IProgramRepository, ProgramRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<ICurriculaRepository, CurriculaRepository>();
builder.Services.AddScoped<IStudentGradeRepository, StudentGradeRepository>();
builder.Services.AddScoped<IEducatorCurriculumRepository, EducatorCurriculumRepository>();
builder.Services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();


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
                ClockSkew = TimeSpan.FromMinutes(2)
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
                    var _cache = context
                                    .HttpContext
                                    .RequestServices
                                    .GetRequiredService<ICacheService>();
                    var isRevoked = await _cache.GetAsync($"Revoked_{TokenToCheck}");
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
    .AddPolicy("Assistant", policy => policy.RequireRole("Assistant"))
    .AddPolicy("EducationalAdvisor", policy => policy.RequireRole("EducationalAdvisor"))
    .AddPolicy("Student", policy => policy.RequireRole("Student"))
    .AddPolicy("DeanViceDeanSecretary", policy =>
    {
        policy.RequireAssertion(ctx =>
        {
            return ctx.User.IsInRole("Dean") ||
                    ctx.User.IsInRole("ViceDean") ||
                    ctx.User.IsInRole("Secretary");
        });
    })
    .AddPolicy("DeanViceDean", policy =>
    {
        policy.RequireAssertion(ctx =>
        {
            return ctx.User.IsInRole("Dean") ||
                    ctx.User.IsInRole("ViceDean");
        });
    });

var app = builder.Build();

//app.UseExceptionHandler();

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowAngular");

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
