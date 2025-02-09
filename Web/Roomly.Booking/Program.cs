using MassTransit;
using Microsoft.EntityFrameworkCore;
using Roomly.Booking.Mappings;
using Roomly.Booking.Services;
using Roomly.Shared.Data;
using Roomly.Shared.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection"), sqlOptions => sqlOptions.MigrationsAssembly("Roomly.Users"));
});

builder.Services.Configure<RabbitOptions>(builder.Configuration.GetSection("RabbitOptions"));

builder.Services.AddAutoMapper(typeof(BookingProfile));

var rabbitOptions = builder.Configuration.GetSection("RabbitOptions").Get<RabbitOptions>();

builder.Services.AddMassTransit(x=>
{ 
    x.UsingRabbitMq((ctx,cfg)=>
    { 
        cfg.Host(rabbitOptions.HostName,"/" , c=> 
        { 
            c.Username(rabbitOptions.UserName);
            c.Password(rabbitOptions.Password); 
        }); 
        cfg.ConfigureEndpoints(ctx); 
    }); 
}); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();