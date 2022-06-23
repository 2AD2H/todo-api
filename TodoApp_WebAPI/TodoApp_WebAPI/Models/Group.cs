using System;
using System.Collections.Generic;

#nullable disable

namespace TodoApp_WebAPI.Models
{
    public partial class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? UserId { get; set; }
    }
}
