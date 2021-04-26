using EntityFrameworkCore.Triggered;
using EntityFrameworkCore.Triggered.Extensions;
using SoftDeleteSample.Models.Traits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftDeleteSample.Triggers
{
    public class SoftDeleteOnRemoval : Trigger<ISoftDeletable>
    {
        readonly ApplicationDbContext _applicationDbContext;

        public SoftDeleteOnRemoval(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public override void BeforeSave(ITriggerContext<ISoftDeletable> context)
        {
            if (context.ChangeType is ChangeType.Deleted)
            {
                context.Entity.DeletedOn = DateTime.UtcNow;
                _applicationDbContext.Entry(context.Entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
        }
    }
}
