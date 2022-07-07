using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace TodoApp_WebAPI.Models
{
    public partial class TodoAppContext : DbContext
    {
        public TodoAppContext()
        {
        }

        public TodoAppContext(DbContextOptions<TodoAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<TaskList> TaskLists { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("TodoDb"));
            }
        }
    }
}
