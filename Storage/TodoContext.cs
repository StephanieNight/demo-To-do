using Microsoft.Extensions.Configuration;
using Storage.Models;
using System.Data.Entity;

namespace Storage
{
    public class TodoContext : DbContext
    {
        public TodoContext(IConfiguration configuration)
            : base(configuration.GetConnectionString("sqldatabase")) { }
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<TodoList> TodoLists { get; set; }
    }
}
