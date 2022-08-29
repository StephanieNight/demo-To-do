using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Storage.Models;
using Todo_App.Models;
using static System.Net.WebRequestMethods;

namespace Todo_App.Controllers
{
    public class HomeController : Controller
    {
        static HttpClient httpClient = new HttpClient();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            //string requestUrl = _config.GetSection("AppSettings").GetSection("APIUrl").Value;
            //string requestKey = _config.GetSection("AppSettings").GetSection("Key").Value;

            string requestUrl = "https://nights-todolist-api.azurewebsites.net/api/5eeace50-5886-421a-95a2-753bb34fa340/lists";
            string requestKey = "orlFiRCWi0RbyRWp5efxc7SdFBY8z6OckeLylyp2EusrAzFubSStrA==";

            try
            {
                httpClient.DefaultRequestHeaders.Add("x-functions-key", requestKey);
                HttpResponseMessage responseMessage = await httpClient.GetAsync(requestUrl);
                string content = await responseMessage.Content.ReadAsStringAsync();
                IEnumerable<TodoList> response = JsonConvert.DeserializeObject<IEnumerable<TodoList>>(content);
                return View(response);
            }
            catch (Exception)
            {
                return View(new List<TodoList>());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
