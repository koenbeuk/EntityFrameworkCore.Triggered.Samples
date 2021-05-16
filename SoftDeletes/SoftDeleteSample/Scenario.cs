using Microsoft.EntityFrameworkCore;
using NuGet.Frameworks;
using ScenarioTests;
using SoftDeleteSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SoftDeleteSample
{
    public partial class Scenario
    {
        [Scenario(NamingPolicy = ScenarioTestMethodNamingPolicy.Test)]
        public static void PlayScenario(ScenarioContext scenario)
        {
            // This represents a basic scenario where soft deletes can be useful.
            // This does not represent production ready code as it currently only soft-deletes entities that are being tracked by the DbContext!

            using var dbContext = new ApplicationDbContext(scenario.TargetName);

            // Some sample models to work with
            var orderItem1 = new OrderItem { DisplayName = "Black kitten" };
            var orderItem2 = new OrderItem { DisplayName = "Gray kitten" };
            var order = new Order { ReferenceName = "X001", OrderItems = new List<OrderItem> { orderItem1, orderItem2 } };
            var user = new User { DisplayName = "John", Orders = new List<Order> { order } };

            // Lets build our initial state
            {
                dbContext.Add(user);
                var result = dbContext.SaveChanges();

                Assert.Equal(4, result);
            }

            scenario.Fact("1. We can soft-delete an order item", () => {
                dbContext.Remove(orderItem1);
                var result = dbContext.SaveChanges();

                Assert.Equal(1, result); // Only our item should have been removed
                Assert.NotNull(orderItem1.DeletedOn); // Instead of a hard-delete it should have received a DeletedOn date
                Assert.Equal(2, dbContext.OrderItems.Count()); // When we query the table we should still have our 2 orderItems
            });

            scenario.Fact("2. We can also soft-delete an order- which in turn will soft-delete its items", () =>
            {
                dbContext.Remove(order);
                var result = dbContext.SaveChanges();

                Assert.Equal(3, result); // Both the order and its items have been affected
                Assert.NotNull(order.DeletedOn); // Expect our order to have been soft-deleted
                Assert.NotNull(orderItem1.DeletedOn); // Expect our items to have been deleted as well
                Assert.NotNull(orderItem2.DeletedOn); 
            });

            scenario.Fact("3. If we delete our user, all entities will be soft deleted", () =>
            {
                dbContext.Remove(user);
                var result = dbContext.SaveChanges();

                Assert.Equal(4, result); // The user, order and items will all have been affected
                Assert.NotNull(user.DeletedOn); // Expect our order to have been soft-deleted
                Assert.NotNull(order.DeletedOn); // Expect our orders to have been deleted as well
                Assert.NotNull(orderItem1.DeletedOn); // Expect our items to have been deleted as well
                Assert.NotNull(orderItem2.DeletedOn);
            });


            // Clear all tracked entities, breaking cascade deletes
            dbContext.ChangeTracker.Clear();
            user = dbContext.Users.First();

            scenario.Fact("4. Even if we only have access to our user, all related entities will still be deleted", () =>
            {
                dbContext.Remove(user);
                var result = dbContext.SaveChanges();

                Assert.Equal(4, result); // The user, order and items will all have been affected
            });
        }
    }
}
