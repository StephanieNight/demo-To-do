using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Storage.Models;
using Storage;
using System.Linq;

namespace Todo_API.functions
{
    public static class AddTodoListItem
    {
        [FunctionName("AddTodoListItem")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "{userid}/{listid}/Additem")] HttpRequest req,
            ILogger log, string userid, string listid)
        {
            // getting the body.
            log.LogInformation("adding an item to the list list");
            var requestbody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation(requestbody);

            var todoitem = JsonConvert.DeserializeObject<TodoItem>(requestbody);
            Guid listguid;

            // Validation
            if (Guid.TryParse(listid, out listguid) == false) { return new BadRequestObjectResult("listis is not valid."); }
            if (string.IsNullOrEmpty(todoitem.Name)) { return new BadRequestObjectResult("Name cannot be empty."); }
            if (string.IsNullOrEmpty(todoitem.Description)) { return new BadRequestObjectResult("Description cannot be empty."); }

            // Adding data.
            todoitem.TodolistId = listguid;
            todoitem.CreatedOnDate = DateTime.Now;

            // writing changes to database.
            var connectionstring = "Server=tcp:nights-demo-server.database.windows.net,1433;Initial Catalog=todolist-prod;Persist Security Info=False;User ID=night-admin;Password=QAZwsx34l;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            var optionsBuilder = new DbContextOptionsBuilder<TodoContext>();
            optionsBuilder.UseSqlServer(connectionstring);
            try
            {
                using (var ctx = new TodoContext(optionsBuilder.Options))
                {
                    var list = ctx.TodoLists.Where(l => l.ASPUser == userid && l.Id == listguid).SingleOrDefault();
                    if (list == null)
                    {
                        return new BadRequestObjectResult("the list was not found for that user");
                    }
                    list.Items.Add(todoitem);                    
                    ctx.SaveChanges();
                    return new OkResult();
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

