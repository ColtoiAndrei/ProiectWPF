using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BazaDateModel
{
    public partial class BazaDateEntitiesModel : DbContext
    {
        public BazaDateEntitiesModel()
            : base("name=BazaDateEntitiesModel")
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }

        public virtual DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Reservations)
                .WithOptional(e => e.Customer)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Movie>()
                .HasMany(e => e.Reservations)
                .WithOptional(e => e.Movie)
                .WillCascadeOnDelete();
        }
    }
}
