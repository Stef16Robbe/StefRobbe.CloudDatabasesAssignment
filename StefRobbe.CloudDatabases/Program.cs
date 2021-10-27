using System.Threading.Tasks;
using DAL;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;

namespace StefRobbe.CloudDatabases
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureServices(services =>
                {
                    // services
                    services.AddScoped<IHousesService, HousesService>();

                    // repos
                    services.AddScoped<IHousesRepository, HousesRepository>();
                    
                    services.AddDbContext<HousesContext>();

                    // cosmosdb setup
                    // services.AddSingleton<ICosmosDbService<House>>(CosmosDbSetup<House>
                    //     .InitializeCosmosClientInstanceAsync("CourseContainer", "/id")
                    //     .GetAwaiter().GetResult());
                })
                .ConfigureFunctionsWorkerDefaults()
                .Build();

            host.Run();
        }
    }
}