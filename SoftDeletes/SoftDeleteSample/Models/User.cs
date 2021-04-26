using SoftDeleteSample.Models.Traits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftDeleteSample.Models
{
    public class User : ISoftDeletable
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public DateTime? DeletedOn { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
