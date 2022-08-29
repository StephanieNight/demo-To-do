using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Storage.Models
{
    public class TodoList
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string ASPUser { get; set; }
        [Display(Name = "Created on Date")]
        [DataType(DataType.Date)]
        public DateTime CreatedOnDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
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
