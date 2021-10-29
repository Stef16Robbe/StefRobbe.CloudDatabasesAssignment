using System.Threading.Tasks;
using Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Services;

namespace StefRobbe.CloudDatabases.Functions
{
    public class DeletePdfFunction
    {
        private readonly ILogger<DeletePdfFunction> _logger;
        private readonly IUserInfoService _userInfoService;

        public DeletePdfFunction(IUserInfoService userInfoService, ILogger<DeletePdfFunction> logger)
        {
            _userInfoService = userInfoService;
            _logger = logger;
        }

        [Function("DeletePDFsTimerTrigger")]
        public async Task DeletePdFs([TimerTrigger("0 0 15 * * *")] MyInfo myTimer, FunctionContext context)
        {
            await _userInfoService.DeleteBlobId();
            _logger.LogInformation("The PDF blobIds for generated mortgages have now been deleted");
        }
    }
}