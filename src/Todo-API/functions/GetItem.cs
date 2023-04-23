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
    public class GetItem
    {
        private TodoContext ctx;

        public GetItem(TodoContext ctx)
        {
            this.ctx = ctx;
        }
        [FunctionName("GetItem")]
        public async Task<IActionResult> Run(
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

                try
                {
                    var list = ctx.TodoItems.Where(l => l.Id == itemGuid).SingleOrDefault();
                    if (list == null) return new BadRequestObjectResult("the list was not found for that user");
                    return new OkObjectResult(list);
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
