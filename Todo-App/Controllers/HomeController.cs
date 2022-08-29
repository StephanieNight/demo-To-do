using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Storage.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Todo_App.Models;

namespace Todo_App.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ILogger<HomeController> logger) : base(logger)
        {
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                httpClient.DefaultRequestHeaders.Add("x-functions-key", requestKey);
                HttpResponseMessage responseMessage = await httpClient.GetAsync(requestBaseUrl+ "5eeace50-5886-421a-95a2-753bb34fa340/lists");
                string content = await responseMessage.Content.ReadAsStringAsync();
                IEnumerable<TodoList> response = JsonConvert.DeserializeObject<IEnumerable<TodoList>>(content);
                return View(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return NotFound();
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
