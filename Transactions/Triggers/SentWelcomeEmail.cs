using EntityFrameworkCore.Triggered;
using EntityFrameworkCore.Triggered.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Transactions.Models;

namespace Transactions.Triggers
{
    public class SentWelcomeEmail : IAfterCommitTrigger<User>
    {
        public Task AfterCommit(ITriggerContext<User> context, CancellationToken cancellationToken)
        {
            // We're not actually sending an email as that's outside of the scope of this demo
            context.Entity.WelcomeEmailSentDate = DateTime.UtcNow;

            return Task.CompletedTask;
        }
    }
}
