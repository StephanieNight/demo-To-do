//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Azure.WebJobs.Extensions.Http;
//using Microsoft.Extensions.Logging;
//using Storage;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Todo_API.functions.Test
//{
//    public static class Test_Database
//    {
//        [FunctionName("TestDatabase")]
//        public static async Task<IActionResult> Run(
//            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
//            ILogger log)
//        {
//            log.LogInformation("C# HTTP trigger function processed a request.");

//            var connectionstring = "Server=tcp:nights-demo-server.database.windows.net,1433;Initial Catalog=todolist-prod;Persist Security Info=False;User ID=night-admin;Password=QAZwsx34l;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

//            var optionsBuilder = new DbContextOptionsBuilder<TodoContext>();
//            optionsBuilder.UseSqlServer(connectionstring);
//            using (var ctx = new TodoContext(optionsBuilder.Options))
//            {
//                return new OkObjectResult($"There is currently {ctx.TodoLists.Count()} List in the database");
//            }         
//        }
//    }
//}
