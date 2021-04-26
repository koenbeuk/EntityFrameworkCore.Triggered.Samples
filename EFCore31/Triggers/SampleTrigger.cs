using EFCore31.Models;
using EntityFrameworkCore.Triggered;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EFCore31.Triggers
{
    public class SampleTrigger : IBeforeSaveTrigger<User>
    {
        public Task BeforeSave(ITriggerContext<User> context, CancellationToken cancellationToken)
        {
            context.Entity.Counter++;
            return Task.CompletedTask;
        }
    }
}
