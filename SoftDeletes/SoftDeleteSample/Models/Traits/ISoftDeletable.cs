using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftDeleteSample.Models.Traits
{
    public interface ISoftDeletable
    {
        public DateTime? DeletedOn { get; set; }
    }
}
