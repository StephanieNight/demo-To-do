using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Storage;
using System.IO;

[assembly: FunctionsStartup(typeof(Todo_API.StartUp))]
namespace Todo_API
{

    public class StartUp : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Snippet for using the local.settings.json file. should be changed to the work with the build in system.
            IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
          
            builder.Services.AddSingleton(config);

            builder.Services.AddTransient<TodoContext>();
            builder.Services.BuildServiceProvider();
        }
    }
}
