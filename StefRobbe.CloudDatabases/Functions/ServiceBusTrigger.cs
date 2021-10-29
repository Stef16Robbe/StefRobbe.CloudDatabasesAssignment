using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services;

namespace StefRobbe.CloudDatabases.Functions
{
    public class ServiceBusTrigger
    {
        private readonly ILogger<ServiceBusTrigger> _logger;
        private readonly IUserInfoService _userInfoService;

        public ServiceBusTrigger(IUserInfoService userInfoService, ILogger<ServiceBusTrigger> logger)
        {
            _userInfoService = userInfoService;
            _logger = logger;
        }

        [Function("ServiceBusTrigger")]
        public async Task ProcessMortgageApplications(
            [ServiceBusTrigger("mortgageapplications", Connection = "ServiceBusConnectionString")]
            string myQueueItem,
            FunctionContext context)
        {
            var userInfoId = JsonConvert.DeserializeObject<string>(myQueueItem);
            await _userInfoService.CreateMortgageApplication(userInfoId);

            _logger.LogInformation("C# ServiceBus queue trigger function processed message: {Message}", myQueueItem);
        }
    }
}