using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace TodoApp_WebAPI.Models
{
    public partial class TaskList
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(150)]
        public string Name { get; set; }
        [Required]
        public int UserId { get; set; }
        public int? GroupId { get; set; }
        public int TaskCount { get; set; }
    }
}
