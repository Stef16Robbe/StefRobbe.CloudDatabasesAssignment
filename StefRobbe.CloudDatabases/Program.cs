using DAL;
using DAL.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                    services.AddScoped<IEmailService, EmailService>();
                    services.AddScoped<IBlobService, BlobService>();

                    // repos
                    services.AddScoped<IHousesRepository, HousesRepository>();
                    services.AddScoped<IEmailRepository, EmailRepository>();

                    services.AddDbContext<HousesContext>();
                    services.AddDbContext<UserContext>();

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