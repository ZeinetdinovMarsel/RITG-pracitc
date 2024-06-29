using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TMS.API.Extentions;
using TMS.Application.Services;
using TMS.Core.Abstractions;
using TMS.DataAccess;
using TMS.DataAccess.Repositories;
using TMS.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors();

builder.Services.AddDbContext<TMSDbContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(TMSDbContext)));
    });



builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddScoped<ITsksService, TsksService>();
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<AdminService>();

builder.Services.AddScoped<ITsksRepository, TsksRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.Configure<AuthorizationOptions>(builder.Configuration.GetSection(nameof(AuthorizationOptions)));
builder.Services.AddApiAuthentification(builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>());


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Lax,
    HttpOnly = HttpOnlyPolicy.Always,
});

app.UseCors(options =>
    options.WithOrigins("http://localhost:3000")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.AddMappedEndpoints();


app.Run();