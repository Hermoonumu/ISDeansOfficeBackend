using DeanInfoSystem.Infrastructure.Repos;
using DeanInfoSystem.Infrastructure.Repos.Interface;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

//Application configurator
var builder = WebApplication.CreateBuilder(args);


//DB connect (postgres db)
builder.Services.AddDbContext<SystemDbContext>(options =>
    options.UseNpgsql(builder.Configuration["ConnectionStrings:PostgreSQL"]));

//Services



//Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();


//API Docs
app.MapScalarApiReference();


using (var scope = app.Services.CreateScope())
{

}

app.Run();
