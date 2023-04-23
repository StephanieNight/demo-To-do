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
    public class GetList
    {
        private TodoContext ctx;
        public GetList(TodoContext ctx)
        {
            this.ctx = ctx;
        }
        [FunctionName("GetList")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "{userid}/{listid}/getList")] HttpRequest req,
            ILogger log, string userid, string listid)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // getting the body.
            log.LogInformation($"getting the list {listid} for the user {userid}");
            var connection = ctx.TodoLists.FirstOrDefault();
            log.LogInformation($"{connection}");

            Guid listGuid;

            // Validation
            if (Guid.TryParse(listid, out listGuid) == false) { return new BadRequestObjectResult("itemid is not valid."); }

            try
            {

                var list = ctx.TodoLists.Where(l => l.ASPUser == userid && l.Id == listGuid).SingleOrDefault();

                if (list == null) return new BadRequestObjectResult("the list was not found for that user");

                var Items = ctx.TodoItems.Where(i => i.TodolistId == list.Id).ToList();

                list.Items.AddRange(Items);

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

