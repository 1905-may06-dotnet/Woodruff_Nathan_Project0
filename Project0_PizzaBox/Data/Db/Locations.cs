using System;
using System.Collections.Generic;

namespace Data.Db
{
    public partial class Locations
    {
        public Locations()
        {
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }
        public string City { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
    }
}
