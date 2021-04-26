using EFCore31.Models;
using EFCore31.Triggers;
using EntityFrameworkCore.Triggered;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore31
{
    public class ApplicationDbContext : TriggeredDbContext
    {
        public ApplicationDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("test");
            optionsBuilder.UseTriggers(triggerOptions =>
            {
                triggerOptions.AddTrigger<SampleTrigger>();
            });
        }

        public DbSet<User> Users { get; set; }
    }
}
