using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Linq;

namespace Storage.Models
{
    public class TodoItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid TodolistId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        [Display(Name = "Created on Date")]
        [DataType(DataType.Date)]
        public DateTime CreatedOnDate { get; set; }

        [Display(Name = "Created on Date")]
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }
        public bool isDone { get; set; }
    }
}
