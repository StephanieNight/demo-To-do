using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Storage;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Todo_API.functions
{
    public class GetTodoListsForUser
    {
        private TodoContext ctx;
        public GetTodoListsForUser(TodoContext ctx)
        {
            this.ctx = ctx;
        }
        [FunctionName("GetTodoListForUser")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "{userid}/lists")] HttpRequest req,
            ILogger log, string userid)
        {
            // getting the body.
            log.LogInformation($"getting lists for user {userid}");

            try
            {
                var lists = ctx.TodoLists.Where(l => l.ASPUser == userid).ToList();
                return new OkObjectResult(lists);

            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return new UnprocessableEntityObjectResult(e.Message);
            }
        }
    }
}

