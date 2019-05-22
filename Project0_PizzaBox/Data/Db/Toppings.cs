using System;
using System.Collections.Generic;

namespace Data.Db
{
    public partial class Toppings
    {
        public Toppings()
        {
            Pizzas = new HashSet<Pizzas>();
        }

        public int Id { get; set; }
        public string T1 { get; set; }
        public string T2 { get; set; }
        public string T3 { get; set; }
        public string T4 { get; set; }
        public string T5 { get; set; }

        public virtual ICollection<Pizzas> Pizzas { get; set; }
    }
}
