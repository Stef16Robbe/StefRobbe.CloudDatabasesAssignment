using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;

namespace Helpers
{
    public static class ResponseHelper
    {
        public static async Task<HttpResponseData> BodyResponse<T>(T t, HttpStatusCode code, HttpRequestData req)
        {
            var json = JsonConvert.SerializeObject(t);
            var response = req.CreateResponse(code);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await response.WriteStringAsync(json);
            return response;
        }

        public static async Task<HttpResponseData> MessageResponse(string message, HttpStatusCode code,
            HttpRequestData req)
        {
            var response = req.CreateResponse(code);
            response.Headers.Add("Content-Type", "text/plain");
            await response.WriteStringAsync(message);
            return response;
        }

        public static HttpResponseData NoContentResponse(HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.NoContent);
            return response;
        }
    }
}