using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Models;

namespace Transactions
{
    public class ApplicationDbContext : DbContext
    {
        readonly DbConnection _connection;

        public ApplicationDbContext(DbConnection connection)
        {
            _connection = connection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connection);
            optionsBuilder.UseTriggers(triggerOptions =>
            {
                triggerOptions.UseTransactionTriggers();
                
                triggerOptions.AddTrigger<Triggers.SentWelcomeEmail>();
            });
        }

        public DbSet<User> Users { get; set; }
    }
}
