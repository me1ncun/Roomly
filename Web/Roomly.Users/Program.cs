using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Roomly.Shared.Auth.Services;
using Roomly.Shared.Data;
using Roomly.Users.Infrastructure.Extensions;
using Roomly.Users.Infrastructure.Handlers;
using Roomly.Users.Infrastructure.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "User API",
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql("Host=postgres;Port=5432;Database=roomly;Username=postgres;Password=postgres;", b => b.MigrationsAssembly("Roomly.Shared")); ;
});

builder.Services.AddScoped<IdentityService>();

builder.Services.AddAutoMapper(typeof(UserProfile));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,
    Secure = CookieSecurePolicy.None
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();