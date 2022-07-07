using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace TodoApp_WebAPI.Models
{
    public partial class Task
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        [Required]
        [StringLength(150)]
        public string Note { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        [Required]
        public int UserId { get; set; }
        public int? ListId { get; set; }
        public bool? IsImportant { get; set; }
        public bool? IsCompleted { get; set; }
        public bool? IsInMyDay { get; set; }
    }
}
