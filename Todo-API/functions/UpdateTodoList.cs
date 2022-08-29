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
    public static class UpdateTodoList
    {
        [FunctionName("UpdateTodoList")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "{userid}/{todolistid}/updatelist")] HttpRequest req,
            ILogger log, string userid, string todolistid)
        {
            // getting the body.
            log.LogInformation($"Updateing todolist : {todolistid}");
            var requestbody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation(requestbody);

            var todoitem = JsonConvert.DeserializeObject<TodoItem>(requestbody);
            Guid listguid;

            // Validation
            if (Guid.TryParse(todolistid, out listguid) == false) { return new BadRequestObjectResult("itemid is not valid."); }
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
                    var list = ctx.TodoLists.Where(l => l.Id == listguid).SingleOrDefault();
                    if (list == null)
                    {
                        return new BadRequestObjectResult("the item was not found for that user");
                    }
                    list.Name = todoitem.Name;
                    list.Description = todoitem.Description;
                    list.CreatedOnDate = todoitem.CreatedOnDate;

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

