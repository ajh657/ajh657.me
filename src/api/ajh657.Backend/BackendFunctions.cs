using System.Net;
using System.Threading.Tasks;
using ajh657.Backend.Cache;
using ajh657.Common.Data.Backend;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ajh657.Backend
{
    public class BackendFunctions
    {
        private readonly ILogger _logger;
        private readonly IStoryCache _storyCache;

        public BackendFunctions(ILoggerFactory loggerFactory, IStoryCache storyCache)
        {
            _logger = loggerFactory.CreateLogger<BackendFunctions>();
            _storyCache = storyCache;
        }

        [Function("ListStories")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {

            var requesterIP = Util.GetIpFromRequestHeaders(req.Headers);
            var userAgent = Util.GetUserAgentFromRequestHeaders(req.Headers);

            _logger.LogInformation("Listing of all stories was requested by {reguesterIP} with user agent string {userAgent}", requesterIP, userAgent);

            var stories = await _storyCache.GetStoriesAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");

            response.WriteString(JsonConvert.SerializeObject(stories));

            return response;
        }
    }
}
