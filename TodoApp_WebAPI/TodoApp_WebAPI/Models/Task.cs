using System;
using System.Collections.Generic;

#nullable disable

namespace TodoApp_WebAPI.Models
{
    public partial class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public int UserId { get; set; }
        public int? ListId { get; set; }
        public bool? IsImportant { get; set; }
        public bool? IsCompleted { get; set; }
        public bool? IsInMyDay { get; set; }
    }
}
