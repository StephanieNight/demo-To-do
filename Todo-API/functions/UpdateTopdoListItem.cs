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
using System.Linq;
using System.Threading.Tasks;

namespace Todo_API.functions
{
    public static class UpdateTopdoListItem
    {
        [FunctionName("UpdateTopdoListItem")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "{userid}/{itemid}/updateitem")] HttpRequest req,
            ILogger log, string userid, string itemid)
        {
            // getting the body.
            log.LogInformation("adding an item to the list list");
            var requestbody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation(requestbody);

            var todoitem = JsonConvert.DeserializeObject<TodoItem>(requestbody);
            Guid itemguid;

            // Validation
            if (Guid.TryParse(itemid, out itemguid) == false) { return new BadRequestObjectResult("itemid is not valid."); }
            if (string.IsNullOrEmpty(todoitem.Name)) { return new BadRequestObjectResult("Name cannot be empty."); }
            if (string.IsNullOrEmpty(todoitem.Description)) { return new BadRequestObjectResult("Description cannot be empty."); }

            // writing changes to database.
            var connectionstring = "Server=tcp:nights-demo-server.database.windows.net,1433;Initial Catalog=todolist-prod;Persist Security Info=False;User ID=night-admin;Password=QAZwsx34l;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            var optionsBuilder = new DbContextOptionsBuilder<TodoContext>();
            optionsBuilder.UseSqlServer(connectionstring);
            try
            {
                using (var ctx = new TodoContext(optionsBuilder.Options))
                {
                    var item = ctx.TodoItems.Where(l => l.Id == itemguid).SingleOrDefault();
                    if (item == null)
                    {
                        return new BadRequestObjectResult("the item was not found for that user");
                    }
                    item.DueDate = todoitem.DueDate;
                    item.Name = todoitem.Name;
                    item.Description = todoitem.Description;
                    item.CreatedOnDate = todoitem.CreatedOnDate;
                    item.DueDate = todoitem.DueDate;
                    item.isDone = todoitem.isDone;
                    ctx.SaveChanges();
                    return new CreatedResult("item", JsonConvert.SerializeObject(todoitem));
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

