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
        protected static string requestUrl = "https://nights-todolist-api.azurewebsites.net/api/5eeace50-5886-421a-95a2-753bb34fa340/lists";
        protected static string requestKey = "orlFiRCWi0RbyRWp5efxc7SdFBY8z6OckeLylyp2EusrAzFubSStrA==";
        protected readonly ILogger _logger;

        public BaseController(ILogger logger)
        {
            _logger = logger;
        }
    }
}
