using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Storage.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace Todo_App.Controllers
{
    public class ListController : BaseController
    {
        Guid ListID;
        public ListController(ILogger<ListController> logger) : base(logger)
        {
        }
        public async Task<IActionResult> Index(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return NotFound();
            }
            ListID = Id;
            try
            {
                var url = requestBaseUrl + $"5eeace50-5886-421a-95a2-753bb34fa340/{Id}/getlist";
                httpClient.DefaultRequestHeaders.Add("x-functions-key", requestKey);
                HttpResponseMessage responseMessage = await httpClient.GetAsync(url);
                string content = await responseMessage.Content.ReadAsStringAsync();
                TodoList response = JsonConvert.DeserializeObject<TodoList>(content);
                if (response == null)
                {
                    return NotFound();
                }
                ViewData["ListName"] = response.Name;
                ViewData["Description"] = response.Description;
                return View(response.Items);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return NotFound();
            }
        }
        public IActionResult CreateList()
        {
            return View(new TodoList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateList([Bind("Name,Description,CreatedOnDate")] TodoList list)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var url = requestBaseUrl + $"5eeace50-5886-421a-95a2-753bb34fa340/Createlist";
                    httpClient.DefaultRequestHeaders.Add("x-functions-key", requestKey);
                    HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync(url,list);
                    string content = await responseMessage.Content.ReadAsStringAsync();
                    TodoList response = JsonConvert.DeserializeObject<TodoList>(content);
                    if (response == null)
                    {
                        return NotFound();
                    }
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                    return NotFound();
                }
               
            }
            return View(list);
        }
        public async Task<IActionResult> EditList(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return NotFound();
            }
            try
            {
                var url = requestBaseUrl + $"5eeace50-5886-421a-95a2-753bb34fa340/{Id}/getlist";
                httpClient.DefaultRequestHeaders.Add("x-functions-key", requestKey);
                HttpResponseMessage responseMessage = await httpClient.GetAsync(url);
                string content = await responseMessage.Content.ReadAsStringAsync();
                TodoList response = JsonConvert.DeserializeObject<TodoList>(content);
                if (response == null)
                {
                    return NotFound();
                }
                return View(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return NotFound();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditList(Guid Id, [Bind("Name,Description,CreatedOnDate")] TodoList list)
        {
            if (Id == Guid.Empty)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var url = requestBaseUrl + $"5eeace50-5886-421a-95a2-753bb34fa340/{Id}/updatelist";
                    httpClient.DefaultRequestHeaders.Add("x-functions-key", requestKey);
                    HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync(url, list);
                 
                    
                    if (responseMessage.IsSuccessStatusCode == false)
                    {
                        return NotFound();
                    }
                    return RedirectToAction("Index","Home");
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                    return NotFound();
                }              
            }
            return View(list);
        }
        public async Task<IActionResult> EditItem(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return NotFound();
            }
            try
            {
                var url = requestBaseUrl + $"5eeace50-5886-421a-95a2-753bb34fa340/{Id}/getlist";
                httpClient.DefaultRequestHeaders.Add("x-functions-key", requestKey);
                HttpResponseMessage responseMessage = await httpClient.GetAsync(url);
                string content = await responseMessage.Content.ReadAsStringAsync();
                TodoList response = JsonConvert.DeserializeObject<TodoList>(content);
                if (response == null)
                {
                    return NotFound();
                }
                return View(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return NotFound();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem(Guid Id, [Bind("Id,TodolistId,Name,Description,CreatedOnDate,DueDate,isDone")] TodoList list)
        {
            if (Id == Guid.Empty)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var url = requestBaseUrl + $"5eeace50-5886-421a-95a2-753bb34fa340/{Id}/updatelist";
                    httpClient.DefaultRequestHeaders.Add("x-functions-key", requestKey);
                    HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync(url, list);


                    if (responseMessage.IsSuccessStatusCode == false)
                    {
                        return NotFound();
                    }
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                    return NotFound();
                }
            }
            return View(list);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateItem([Bind("Id,TodolistId,Name,Description,CreatedOnDate,DueDate,isDone")] TodoList list)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var url = requestBaseUrl + $"5eeace50-5886-421a-95a2-753bb34fa340/Createlist";
                    httpClient.DefaultRequestHeaders.Add("x-functions-key", requestKey);
                    HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync(url, list);
                    string content = await responseMessage.Content.ReadAsStringAsync();
                    TodoList response = JsonConvert.DeserializeObject<TodoList>(content);
                    if (response == null)
                    {
                        return NotFound();
                    }
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                    return NotFound();
                }

            }
            return View(list);
        }
    }
}
