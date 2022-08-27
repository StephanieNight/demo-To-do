using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Storage;
using Storage.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Todo_API.functions
{
    public static class AddTodoList
    {
        [FunctionName("AddTodoList")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "{userid}/Createlist")] HttpRequest req,
            ILogger log, string userid)
        {
            // getting the body.
            log.LogInformation("Creating a new list");
            var requestbody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation(requestbody);

            var todolist = JsonConvert.DeserializeObject<TodoList>(requestbody);
            // Validation.
            if (string.IsNullOrEmpty(todolist.Name)) { return new BadRequestObjectResult("Name cannot be empty."); }
            if (string.IsNullOrEmpty(todolist.Description)) { return new BadRequestObjectResult("Description cannot be empty."); }

            // Adding data.
            todolist.ASPUser = userid;
            todolist.CreatedOnDate = DateTime.Now;

            // writing changes to database.
            var connectionstring = "Server=tcp:nights-demo-server.database.windows.net,1433;Initial Catalog=todolist-prod;Persist Security Info=False;User ID=night-admin;Password=QAZwsx34l;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            var optionsBuilder = new DbContextOptionsBuilder<TodoContext>();
            optionsBuilder.UseSqlServer(connectionstring);
            try
            {
                using (var ctx = new TodoContext(optionsBuilder.Options))
                {
                    ctx.Add(todolist);
                    ctx.SaveChanges();
                    return new CreatedResult("list", JsonConvert.SerializeObject(todolist));
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return new UnprocessableEntityObjectResult(e.Message);
            }
        }
    }
}

