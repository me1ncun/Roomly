using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Roomly.Rooms.Helpers;
using Roomly.Shared.Data;
using Roomly.Shared.Data.Entities;
using Roomly.Users.Infrastructure.Auth;
using Roomly.Users.Infrastructure.Extensions;
using Roomly.Users.Infrastructure.Mappings;
using Roomly.Users.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection"), sqlOptions => sqlOptions.MigrationsAssembly("Roomly.Users"));
});

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddAutoMapper(typeof(UserProfile));

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)))
    .AddScoped<JwtProvider>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<RoleManager<IdentityRole>>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
    await app.SeedRolesAsync();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();