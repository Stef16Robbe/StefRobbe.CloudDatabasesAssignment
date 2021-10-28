using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services;

namespace StefRobbe.CloudDatabases.Functions
{
    public class HousesFunctions
    {
        private readonly IHousesService _housesService;
        private readonly ILogger<HousesFunctions> _logger;

        public HousesFunctions(ILogger<HousesFunctions> logger, IHousesService housesService)
        {
            _logger = logger;
            _housesService = housesService;
        }

        [Function("GetHouses")]
        public async Task<HttpResponseData> GetHouses(
            [HttpTrigger(AuthorizationLevel.Function, "get")]
            HttpRequestData req, int index, int maxItems, float priceFrom = 0f, float priceTo = -1f)
        {
            var houses = _housesService.GetHousesPaginated(index, maxItems, priceFrom, priceTo);

            var json = JsonConvert.SerializeObject(houses);
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await response.WriteStringAsync(json);

            return response;
        }
    }
}