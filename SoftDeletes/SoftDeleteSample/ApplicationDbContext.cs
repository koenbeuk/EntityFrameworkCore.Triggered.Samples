using Microsoft.EntityFrameworkCore;
using SoftDeleteSample.Models.Traits;
using SoftDeleteSample.Models;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace SoftDeleteSample
{
    public class ApplicationDbContext : DbContext
    {
        readonly string _databaseName;

        public ApplicationDbContext(string databaseName)
        {
            _databaseName = databaseName;
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Order> Orers { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(_databaseName);
            optionsBuilder.UseTriggers(triggerOptions =>
            {
                triggerOptions.AddAssemblyTriggers();
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(x => x.Orders)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasMany(x => x.OrderItems)
                .WithOne(x => x.Order)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
