using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Storage.Models
{
    public class TodoList
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public IdentityUser ASPUser { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public virtual List<TodoItem> Items { get; set; } = new List<TodoItem>();
        [NotMapped]
        public bool IsDone
        {
            get
            {
                foreach (var item in Items)
                {
                    if (item.isDone == false) return false;
                }
                return true;
            }
        }
    }
}
