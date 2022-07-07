using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp_WebAPI.Models
{
    public class Account
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
