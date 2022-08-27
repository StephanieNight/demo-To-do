using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Storage.Models
{
    public class TodoItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid TodolistId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public DateTime? DueDate { get; set; }
        public bool isDone { get; set; }

    }
}
