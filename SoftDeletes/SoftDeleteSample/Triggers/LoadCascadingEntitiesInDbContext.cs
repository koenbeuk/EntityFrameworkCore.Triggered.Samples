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
    public class LoadCascadingEntitiesInDbContext : Trigger<ISoftDeletable>
    {
        readonly ApplicationDbContext _applicationDbContext;

        public LoadCascadingEntitiesInDbContext(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public override void BeforeSave(ITriggerContext<ISoftDeletable> context)
        {
            if (context.ChangeType is ChangeType.Deleted)
            {
                var entry = _applicationDbContext.Entry(context.Entity);

                foreach (var navigationMetadata in entry.Metadata.GetNavigations())
                {
                    if (navigationMetadata.ForeignKey.DeleteBehavior == Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade)
                    {
                        var navigation = entry.Navigation(navigationMetadata.PropertyInfo.Name);

                        if (!navigation.IsLoaded)
                        {
                            navigation.Load();
                        }
                    }
                }

                _applicationDbContext.ChangeTracker.CascadeChanges();
            }
        }
    }
}
