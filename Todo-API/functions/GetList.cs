using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Storage;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Todo_API.functions
{
    public static class GetList
    {
        [FunctionName("GetList")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "{userid}/{listid}/getList")] HttpRequest req,
            ILogger log, string userid, string listid)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            {
                // getting the body.
                log.LogInformation($"getting the list {listid} for the user {userid}");


                Guid listGuid;

                // Validation
                if (Guid.TryParse(listid, out listGuid) == false) { return new BadRequestObjectResult("itemid is not valid."); }

                // getting info from database.
                var connectionstring = "Server=tcp:nights-demo-server.database.windows.net,1433;Initial Catalog=todolist-prod;Persist Security Info=False;User ID=night-admin;Password=QAZwsx34l;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                var optionsBuilder = new DbContextOptionsBuilder<TodoContext>();
                optionsBuilder.UseSqlServer(connectionstring);
                try
                {
                    using (var ctx = new TodoContext(optionsBuilder.Options))
                    {
                        var list = ctx.TodoLists.Where(l => l.ASPUser == userid && l.Id == listGuid).SingleOrDefault();
                        
                        if (list == null) return new BadRequestObjectResult("the list was not found for that user");

                        var Items = ctx.TodoItems.Where(i => i.TodolistId == list.Id).ToList();

                        list.Items.AddRange(Items);

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
