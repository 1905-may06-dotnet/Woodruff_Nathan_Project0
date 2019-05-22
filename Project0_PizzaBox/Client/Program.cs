using System;
using System.Collections.Generic;
using Data.Db;
using System.Linq;

namespace Client
{
    //Pizza Application
    class Program
    {
        //  Prompts user for information and posts records via a database context.
        static void Main(string[] args)
        {

            //Create User object and request credentials
            User hello = new User();
            hello.GetCredentials();

            //Create Location object and request location
            Location loc = new Location();
            loc.SelectLocation();

            //Create Pizza object and request specifications
            Pizza pizza = new Pizza();
            pizza.MakePizza();


            //Console Output Region
            #region Pizza Output
            Console.WriteLine("");
            Console.WriteLine("You have finished building your pizza.");
            Console.WriteLine($"Your pizza is size {pizza.size}.");
            Console.WriteLine($"Your pizza has {pizza.crust} crust.");
            Console.WriteLine($"You have selected {pizza.toppings.Count} toppings: ");
            Console.WriteLine($"Your total is {pizza.cost}");
            foreach (var i in pizza.toppings)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine($"Your total is {pizza.cost}");

            Console.WriteLine($"Your store in {loc.city}");
            Console.WriteLine("");
            #endregion Pizza Output


            //Instantiate the database context and add records
            #region Database

            var db = new PizzaContext();

            Data.Db.Toppings topping_obj = new Data.Db.Toppings()
            {
                T1 = pizza.toppings[0],
                T2 = pizza.toppings[1],
                T3 = pizza.toppings[2],
                T4 = pizza.toppings[3],
                T5 = pizza.toppings[4]
            };

            Data.Db.Pizzas pizza_obj = new Data.Db.Pizzas()
            {
                Size = pizza.size,
                Crust = pizza.crust,
                Cost = pizza.cost,
                Topping = topping_obj
            };

            Data.Db.Users user_obj = new Data.Db.Users()
            {
                Username = hello.username,
                Password = hello.password
            };

            Data.Db.Locations location_obj = new Data.Db.Locations()
            {
                City = loc.city
            };

            Data.Db.Orders order_obj = new Data.Db.Orders()
            {
                Pizza = pizza_obj,
                Username = user_obj,
                Location = location_obj,
                OrderTime = DateTime.Now
            };


            //Add Toppings
            db.Toppings.Add(topping_obj);

            //Add Pizza
            db.Pizzas.Add(pizza_obj);

            //Add Order
            db.Orders.Add(order_obj);

            //Add User
            db.Users.Add(user_obj);

            var count = db.SaveChanges();
            Console.WriteLine($"{count} records saved to database");
            #endregion

        }

        //Contains Domain logic for Location selection
        public class Location
        {
            #region Location Properties
            public int id;
            public string city;
            public List<string> locations = new List<string> { "dallas", "ft. worth", "arlington", "plano", "irving", "garland" };
            public int pizza_id;
            #endregion

            //User Selects a valid location from list in properties
            public void SelectLocation()
            {
                Console.WriteLine("What is your city?");
                Console.WriteLine("");
                foreach (var i in locations)
                {
                    Console.WriteLine(i);
                }

                Console.WriteLine("");
                var tempCity = Console.ReadLine().ToLower();

                if (!locations.Contains(tempCity))
                {
                    Console.Clear();
                    Console.WriteLine("Please select a valid service area");
                    SelectLocation();
                }
                else
                {
                    this.city = tempCity;
                }
                Console.Clear();
            }
        }

        //Contains Domain logic for creating Pizza objects
        public class Pizza
        {
            #region Pizza Properties
            public int id;
            public string crust;
            public string size;
            public decimal cost = 0M;
            public static int maxToppings = 5;
            public List<string> toppings = new List<string>();
            public int topping_id;
            #endregion

            //Creates a Pizza object with user specified inputs
            public Pizza MakePizza()
            {
                // Choose Size
                SelectSize();
                void SelectSize()
                {
                    Console.WriteLine("What size do you want? [Small, Medium, Large]");
                    string tempSize = Console.ReadLine().ToLower();
                    if (!tempSize.Equals("small")
                        && !tempSize.Equals("medium")
                        && !tempSize.Equals("large"))
                    {
                        Console.Clear();
                        Console.WriteLine("Please select a valid size.");
                        Console.WriteLine("");

                        SelectSize();
                    }
                    else
                    {
                        this.size = tempSize;
                    }

                    switch (this.size)
                    {
                        case "small":
                            this.cost += 1.50M;
                            break;
                        case "medium":
                            this.cost += 3.50M;
                            break;
                        case "large":
                            this.cost += 5.00M;
                            break;
                    }
                }

                Console.Clear();

                // Choose Crust
                SelectCrust();
                void SelectCrust()
                {
                    Console.WriteLine("What kind of crust do you want? [Crispy, Fluffy, Cheese-Stuffed]");
                    string tempCrust = Console.ReadLine().ToLower();
                    if (!tempCrust.Equals("crispy")
                        && !tempCrust.Equals("fluffy")
                        && !tempCrust.Equals("cheese-stuffed"))
                    {
                        Console.Clear();
                        Console.WriteLine("Please select a valid crust type.");
                        SelectCrust();
                    }
                    else
                    {
                        this.crust = tempCrust;
                    }

                    switch (this.crust)
                    {
                        case "crispy":
                            this.cost += .50M;
                            break;
                        case "fluffy":
                            this.cost += .50M;
                            break;
                        case "cheese-stuffed":
                            this.cost += 1.50M;
                            break;
                    }
                }

                Console.Clear();

                // Choose Toppings
                SelectToppings();
                void SelectToppings()
                {
                    Console.WriteLine("What toppings do you want on your pizza? \n" +
                        "You start with sauce and cheese. Select up to 3 more. \n" +
                        "Input 'break' when you are done adding toppings.\n");

                    this.toppings.Add("sauce");
                    this.toppings.Add("cheese");

                    for (int i = this.toppings.Count; i < Pizza.maxToppings; i++)
                    {
                        string tempTopping = Console.ReadLine().ToLower();
                        if (tempTopping == "break")
                        {
                            for (int j = this.toppings.Count; j < Pizza.maxToppings; j++)
                            {
                                this.toppings.Add(null);
                            }

                            Console.Clear();
                            break;
                        }
                        else
                        {
                            this.toppings.Add(tempTopping);
                        }
                    }
                    this.cost += this.toppings.Count * 1.50M;
                }

                return this;
            }

        }

        //Contains Domain logic for internal transactions
        public class Business
        {
            //Enables internal business transactions such as inventory tracking as a function of orders placed.

            #region Business Properties
            public string ingredient;
            public Dictionary<string, int> ingredients = new Dictionary<string, int>();
            public List<object> orders = new List<object>();
            #endregion

            void SetInventory(string ingredient, int quantity)
            {
                ingredients.Add(ingredient, quantity);
            }
            void GetInventory(string ingredient)
            {
                int val = ingredients[ingredient];
                Console.WriteLine(val);
            }

        }

        //Contains Domain logic for User handling and authentication
        public class User
        {
            #region User Properties
            public int id;
            public string username;
            public string password;
            public int pizza_id;
            #endregion

            PizzaContext db = new PizzaContext();

            public void GetCredentials()
            {
                Console.WriteLine("Enter a username: ");
                this.username = Console.ReadLine().ToString();
                Console.WriteLine("Enter a password: ");
                this.password = Console.ReadLine().ToString();

                var returningUser = db.Users
                .Where<Users>(u => u.Username == this.username).FirstOrDefault();

                if (returningUser == null)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Welcome to PizzaBox application!");
                    Console.WriteLine("");
                }
                else
                {
                    this.CheckTime();
                }

            }
            public void CheckTime()
            {
                var returningUser = db.Users
                    .Where<Users>(u => u.Username == this.username).FirstOrDefault();

                var checkOrder = db.Orders
                    .Where<Orders>(u => u.UsernameId == returningUser.Id).FirstOrDefault();

                var timeQuery = db.Orders.OrderByDescending(order => order.OrderTime).FirstOrDefault();

                var referenceTime = timeQuery.OrderTime.AddHours(2.0);

                var timeLimit = DateTime.Compare(DateTime.Now, referenceTime); //if timeLimit >0 go

                if (returningUser != null
                    && timeLimit < 0)
                {
                    Console.Clear();
                    Console.WriteLine($"You can't place anymore orders until {referenceTime}");
                    Console.WriteLine("");
                    Console.WriteLine("Press ESC to exit.");
                    do
                    {
                    } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

                    this.GetCredentials();
                }
            }
        }
    }
}