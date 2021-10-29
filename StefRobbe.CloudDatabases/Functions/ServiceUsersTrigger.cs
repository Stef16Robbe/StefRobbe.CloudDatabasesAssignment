using System;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Services;

namespace StefRobbe.CloudDatabases.Functions
{
    public class ServiceUsersTrigger
    {
        private readonly IUserInfoService _userInfoService;

        public ServiceUsersTrigger(IUserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        [Function("ServiceUsers")]
        public async Task ServiceUsers([TimerTrigger("0 59 23 * * *")] MyInfo myTimer, FunctionContext context)
        {
            var buyerInfos = await _userInfoService.GetAllUserInfo();

            var serviceBusConnectString = Environment.GetEnvironmentVariable("ServiceBusConnectionString");
            var queueName = Environment.GetEnvironmentVariable("QueueName");

            if (!string.IsNullOrEmpty(queueName))
            {
                IQueueClient client = new QueueClient(serviceBusConnectString, queueName);

                // Send buyerInfo ids to the service bus so the listener can process the requests one at a time.
                foreach (var buyerInfo in buyerInfos)
                {
                    var messageBody = JsonConvert.SerializeObject(buyerInfo.id);
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                    await client.SendAsync(message);
                }
            }
        }
    }
}