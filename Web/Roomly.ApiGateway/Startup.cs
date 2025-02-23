using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

public class Startup
{
    public IConfiguration Configuration { get; set; }
    
    public void ConfigureServices(IServiceCollection services)
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("configuration.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        
        services.AddOcelot(Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseOcelot().Wait();
    }
}