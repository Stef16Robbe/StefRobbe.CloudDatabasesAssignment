using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services;

namespace StefRobbe.CloudDatabases
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
            HttpRequestData req, float priceFrom, float priceTo)
        {
            // var house = await _housesService.CreateHouse(new House());
            var houses = await _housesService.GetHousesPaginated(priceFrom, priceTo);

            var json = JsonConvert.SerializeObject(houses);
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await response.WriteStringAsync(json);

            return response;
        }
    }
}