using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Data.Db
{
    public partial class PizzaContext : DbContext
    {
        public PizzaContext()
        {
        }

        public PizzaContext(DbContextOptions<PizzaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Locations> Locations { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Pizzas> Pizzas { get; set; }
        public virtual DbSet<Toppings> Toppings { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(DbConnection.Connection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Locations>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.OrderTime).HasColumnName("order_time");

                entity.Property(e => e.PizzaId).HasColumnName("pizza_id");

                entity.Property(e => e.UsernameId).HasColumnName("username_id");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK__Orders__location__5535A963");

                entity.HasOne(d => d.Pizza)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PizzaId)
                    .HasConstraintName("FK__Orders__pizza_id__5441852A");

                entity.HasOne(d => d.Username)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UsernameId)
                    .HasConstraintName("FK__Orders__username__534D60F1");
            });

            modelBuilder.Entity<Pizzas>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasColumnType("money");

                entity.Property(e => e.Crust)
                    .IsRequired()
                    .HasColumnName("crust")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Size)
                    .IsRequired()
                    .HasColumnName("size")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ToppingId).HasColumnName("topping_id");

                entity.HasOne(d => d.Topping)
                    .WithMany(p => p.Pizzas)
                    .HasForeignKey(d => d.ToppingId)
                    .HasConstraintName("FK__Pizzas__topping___4CA06362");
            });

            modelBuilder.Entity<Toppings>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.T1)
                    .IsRequired()
                    .HasColumnName("t1")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.T2)
                    .IsRequired()
                    .HasColumnName("t2")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.T3)
                    .HasColumnName("t3")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.T4)
                    .HasColumnName("t4")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.T5)
                    .HasColumnName("t5")
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
