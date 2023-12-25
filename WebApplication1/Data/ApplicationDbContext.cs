using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace WebApplication1.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employees> Employees { get; set; }
        public DbSet<PassportApplication> PassportApplication { get; set; }
        public DbSet<PassportServices> PassportServices { get; set; }
        public DbSet<PersonPassports> PersonPassports { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<PlacesOfBirth> PlacesOfBirth { get; set; }
        public DbSet<RegistrationPlaces> RegistrationPlaces { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PersonConfiguration());

            modelBuilder.ApplyConfiguration(new PersonPassportsConfiguration());
            modelBuilder.ApplyConfiguration(new PassportApplicationConfiguration());


        }

    }

}
