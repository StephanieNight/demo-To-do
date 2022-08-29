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
using Storage;
using System.Linq;

namespace Todo_API.functions
{
    public static class GetItem
    {
        [FunctionName("GetItem")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "{userid}/{itemid}/getitem")] HttpRequest req,
            ILogger log, string userid, string itemid)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            {
                // getting the body.
                log.LogInformation($"getting the item {itemid} for the user {userid}");


                Guid itemGuid;

                // Validation
                if (Guid.TryParse(itemid, out itemGuid) == false) { return new BadRequestObjectResult("itemid is not valid."); }

                // getting info from database.
                var connectionstring = "Server=tcp:nights-demo-server.database.windows.net,1433;Initial Catalog=todolist-prod;Persist Security Info=False;User ID=night-admin;Password=QAZwsx34l;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                var optionsBuilder = new DbContextOptionsBuilder<TodoContext>();
                optionsBuilder.UseSqlServer(connectionstring);
                try
                {
                    using (var ctx = new TodoContext(optionsBuilder.Options))
                    {
                        var list = ctx.TodoItems.Where(l =>  l.Id == itemGuid).SingleOrDefault();
                        if (list == null) return new BadRequestObjectResult("the list was not found for that user");
                        return new OkObjectResult(list);
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
}
