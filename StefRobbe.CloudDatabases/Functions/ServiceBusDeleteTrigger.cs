using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json;
using Services;

namespace StefRobbe.CloudDatabases.Functions
{
    public class ServiceBusDeleteTrigger
    {
        private readonly IUserInfoService _userInfoService;

        public ServiceBusDeleteTrigger(IUserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        [Function("ServiceBusDeleteTrigger")]
        public async Task DeleteTrigger(
            [ServiceBusTrigger("deletequeue", Connection = "ServiceBusConnectionString")] string myQueueItem,
            FunctionContext context)
        {
            var userInfoBlobId = JsonConvert.DeserializeObject<string>(myQueueItem);
            await _userInfoService.DeleteMortgage(userInfoBlobId);
        }
    }
}