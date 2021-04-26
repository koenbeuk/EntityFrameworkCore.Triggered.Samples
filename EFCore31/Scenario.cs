using EFCore31.Models;
using ScenarioTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EFCore31
{
    public partial class Scenario
    {
        [Scenario(NamingPolicy = ScenarioTestMethodNamingPolicy.Test)]
        public void PlayScenario(ScenarioContext scenario)
        {
            // ApplicationDbContext extends from TriggeredDbContext which is required in the v1 branch of EntityFrameworkCore.Triggered
            using var dbContext = new ApplicationDbContext();
            var sampleUser = new User { Counter = 1 };

            dbContext.Users.Add(sampleUser);
            dbContext.SaveChanges();

            scenario.Fact("1. Counter was increased by trigger", () =>
            {
                Assert.Equal(2, sampleUser.Counter);
            });
        }
    }
}
