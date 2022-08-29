using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Todo_App.Controllers
{
    public abstract class BaseController : Controller
    {

        //string requestUrl = _config.GetSection("AppSettings").GetSection("APIUrl").Value;
        //string requestKey = _config.GetSection("AppSettings").GetSection("Key").Value;

        protected static HttpClient httpClient = new HttpClient();
        //protected static string requestBaseUrl = "https://nights-todolist-api.azurewebsites.net/api/";
        protected static string requestBaseUrl = "http://localhost:7110/api/";
        protected static string requestKey = "orlFiRCWi0RbyRWp5efxc7SdFBY8z6OckeLylyp2EusrAzFubSStrA==";
        protected readonly ILogger _logger;

        public BaseController(ILogger logger)
        {
            _logger = logger;
        }
    }
}
