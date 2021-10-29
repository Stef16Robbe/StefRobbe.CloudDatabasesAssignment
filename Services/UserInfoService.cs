using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL;
using DAL.Helpers;
using Domain;
using Helpers;

namespace Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IBlobService _blobService;
        private readonly IUserInfoRepository _userInfoRepository;

        public UserInfoService(IUserInfoRepository userInfoRepository, IBlobService blobService)
        {
            _blobService = blobService;
            _userInfoRepository = userInfoRepository;
        }

        public async Task<UserInfo> CreateUserInfo(UserInfo userInfo)
        {
            return await _userInfoRepository.CreateUserInfo(userInfo);
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