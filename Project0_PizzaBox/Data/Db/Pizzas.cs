using System;
using System.Collections.Generic;

namespace Data.Db
{
    public partial class Pizzas
    {
        public Pizzas()
        {
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }
        public string Size { get; set; }
        public string Crust { get; set; }
        public decimal Cost { get; set; }
        public int? ToppingId { get; set; }

        public virtual Toppings Topping { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
