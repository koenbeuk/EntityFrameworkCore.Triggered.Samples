using EntityFrameworkCore.Triggered;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ScenarioTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Models;
using Xunit;

namespace Transactions
{
    public partial class Scenario
    {
        [Scenario(NamingPolicy = ScenarioTestMethodNamingPolicy.Test)]
        public async Task PlayScenario(ScenarioContext scenario)
        {
            // We need to manually manage a SqlLite connection if we want to work with inmemory databases
            // see: https://docs.microsoft.com/en-us/ef/core/testing/sqlite
            using var connection = new SqliteConnection($"Filename=:memory:");
            connection.Open();

            // Ensure that the database has been created
            using var dbContext = new ApplicationDbContext(connection);
            dbContext.Database.EnsureCreated();
            
            var user = new User { EmailAddress = "john@jane.doe" };

            // We start a transaction manually
            using var transaction = dbContext.Database.BeginTransaction();
            // We also need to manually start a triggerSession as EFCore's DbContext is not aware of application controlled transactions
            using var triggerSession = dbContext.GetService<ITriggerService>().CreateSession(dbContext);

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            scenario.Fact("1. User has been added to the database", () =>
            {
                Assert.NotEqual(0, user.Id); // User has been assigned an Id
            });

            scenario.Fact("2. A welcome email has not yet been sent", () =>
            {
                Assert.Null(user.WelcomeEmailSentDate);
            });

            transaction.Commit();
            await triggerSession.RaiseAfterCommitTriggers();

            scenario.Fact("3. A welcome email has been sent after committing the transaction", () =>
            {
                Assert.NotNull(user.WelcomeEmailSentDate);
            });
        }
    }
}
