using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Domain;
using Helpers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services;

namespace StefRobbe.CloudDatabases.Functions
{
    public class UserFunctions
    {
        private readonly ILogger<UserFunctions> _logger;
        private readonly IUserInfoService _userInfoService;

        public UserFunctions(ILogger<UserFunctions> logger, IUserInfoService userInfoService)
        {
            _logger = logger;
            _userInfoService = userInfoService;
        }

        [Function("CreateUserInfo")]
        public async Task<HttpResponseData> CreateUserInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            try
            {
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(content);

                return await ResponseHelper.BodyResponse(await _userInfoService.CreateUserInfo(userInfo),
                    HttpStatusCode.Created, req);
            }
            catch (Exception e)
            {
                _logger.LogError("{Error}", e.Message);
                return await ResponseHelper.MessageResponse("Oops! Something went wrong.", HttpStatusCode.BadRequest,
                    req);
            }
        }
    }
}