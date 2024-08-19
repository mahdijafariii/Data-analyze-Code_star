using AnalysisData.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class ConsoleStartup
{
    public ConsoleStartup()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
        Configuration = builder.Build();
        

        //.. for test
        Console.WriteLine(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql("Host=localhost;Database=mohaymen;Username=postgres;Password=12345;Timeout=300").UseLazyLoadingProxies();
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
   
    }
}
