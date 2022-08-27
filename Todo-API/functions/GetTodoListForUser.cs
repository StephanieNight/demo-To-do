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
    public static class GetTodoListForUser
    {
        [FunctionName("GetTodoListForUser")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "{userid}/lists")] HttpRequest req,
            ILogger log, string userid)
        {
            // getting the body.
            log.LogInformation($"getting lists for user {userid}");
            
            // getting info from database.
            var connectionstring = "Server=tcp:nights-demo-server.database.windows.net,1433;Initial Catalog=todolist-prod;Persist Security Info=False;User ID=night-admin;Password=QAZwsx34l;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            var optionsBuilder = new DbContextOptionsBuilder<TodoContext>();
            optionsBuilder.UseSqlServer(connectionstring);
            try
            {
                using (var ctx = new TodoContext(optionsBuilder.Options))
                {
                    var lists = ctx.TodoLists.Where(l => l.ASPUser == userid).ToList();
                    return new OkObjectResult(lists);
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

