using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
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
    public class UpdateTodoList
    {
        private TodoContext ctx;
        public UpdateTodoList(TodoContext ctx)
        {
            this.ctx = ctx;
        }
        [FunctionName("UpdateTodoList")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "{userid}/{todolistid}/updatelist")] HttpRequest req,
            ILogger log, string userid, string todolistid)
        {
            //getting the body.
            log.LogInformation($"Updateing todolist : {todolistid}");
            var requestbody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation(requestbody);

            var todoitem = JsonConvert.DeserializeObject<TodoItem>(requestbody);
            Guid listguid;

            //Validation
            if (Guid.TryParse(todolistid, out listguid) == false) { return new BadRequestObjectResult("itemid is not valid."); }
            if (string.IsNullOrEmpty(todoitem.Name)) { return new BadRequestObjectResult("Name cannot be empty."); }
            if (string.IsNullOrEmpty(todoitem.Description)) { return new BadRequestObjectResult("Description cannot be empty."); }

            try
            {

                var list = ctx.TodoLists.Where(l => l.Id == listguid).SingleOrDefault();
                if (list == null)
                {
                    return new BadRequestObjectResult("the item was not found for that user");
                }
                list.Name = todoitem.Name;
                list.Description = todoitem.Description;

                ctx.SaveChanges();
                return new CreatedResult("item", JsonConvert.SerializeObject(todoitem));

            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return new UnprocessableEntityObjectResult(e.Message);
            }
        }
    }
}

