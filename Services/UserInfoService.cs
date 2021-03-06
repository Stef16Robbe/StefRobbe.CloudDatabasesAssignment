using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Helpers;
using Domain;
using Helpers;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IBlobService _blobService;
        private readonly ILogger<UserInfoService> _logger;
        private readonly IUserInfoRepository _userInfoRepository;

        public UserInfoService(IUserInfoRepository userInfoRepository, IBlobService blobService,
            ILogger<UserInfoService> logger)
        {
            _blobService = blobService;
            _userInfoRepository = userInfoRepository;
            _logger = logger;
        }

        public async Task<UserInfo> CreateUserInfo(UserInfo userInfo)
        {
            return await _userInfoRepository.CreateUserInfo(userInfo);
        }

        public async Task DeleteBlobId()
        {
            var userInfos = await GetAllUserInfo();

            var serviceBusConnectString = Environment.GetEnvironmentVariable("ServiceBusConnectionString");
            var queueName = Environment.GetEnvironmentVariable("DeleteQueueName");

            if (!string.IsNullOrEmpty(queueName))
            {
                IQueueClient client = new QueueClient(serviceBusConnectString, queueName);

                // Send userInfo ids to the service bus so the listener can process the requests one at a time.
                foreach (var userInfo in userInfos)
                {
                    var messageBody = JsonConvert.SerializeObject(userInfo.BlobId);
                    userInfo.BlobId = "";

                    await _userInfoRepository.UpdateUserInfo(userInfo);
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                    await client.SendAsync(message);
                }
            }
        }

        public async Task DeleteMortgage(string userInfoBlobId)
        {
            await _blobService.DeleteBlobFromServer(userInfoBlobId);
            _logger.LogInformation("PDF blob has been deleted from blob storage");
        }

        public async Task CreateMortgageApplication(string myQueueItem)
        {
            var userInfo = await _userInfoRepository.GetUserInfoById(myQueueItem);
            var amountToBorrow = userInfo.YearlyIncome * 10;
            var pdf = PDFUtil.CreatePDF(userInfo, amountToBorrow);
            var fileName = await _blobService.CreateFile(Convert.ToBase64String(pdf), Guid.NewGuid() + ".pdf");
            userInfo.BlobId = fileName;
            await _userInfoRepository.AddBlobId(userInfo);
        }

        public async Task<IEnumerable<UserInfo>> GetAllUserInfo()
        {
            return await _userInfoRepository.GetAllUserInfo();
        }

        public async Task<UserInfo> GetUserInfoById(string id)
        {
            return await _userInfoRepository.GetUserInfoById(id);
        }
    }
}