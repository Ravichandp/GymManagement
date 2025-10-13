using GymManagement.API.Authentication.Models.Domain;
using GymManagement.API.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.API.Authentication.Data
{
    // Fix: Inherit only from IdentityDbContext<ApplicationUser>
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base (options)
        {
            
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);

        //    if (!optionsBuilder.IsConfigured)
        //    {

        //        // Build configuration
        //        var configuration = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json")
        //        .Build();

        //        // Configure DbContext options
        //        optionsBuilder.UseSqlServer(configuration.GetConnectionString("AuthenticationConnectionString"));
        //    }
        //}

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var customers = new List<Customer>()
            {
                new Customer()
                {
                    CustomerId = Guid.Parse("4cdf5391-df19-445f-ae2d-0b1474acf277"),
                    FirstName="Ravichand",
                    MiddleName = "Balaswami",
                    LastName = "Prathipati",
                    Gender ="Male",
                    Age = 32,
                    City ="Hyderabad",
                    UserName="Ravichand",
                    Email="ravi@gmail.com",
                    Password="Ravi@123"
                },
                   new Customer()
                {
                    CustomerId = Guid.Parse("cf1efe89-f48d-4200-b0ec-df4a666a85bf"),
                    FirstName="Sowjanya",
                    MiddleName = "Ravichand",
                    LastName = "Kambhampati",
                    Gender ="FeMale",
                    Age = 32,
                    City ="Hyderabad",
                    UserName="Sowji",
                    Email="sowji@gmail.com",
                    Password="Sowji@123"
                }
            };

            //Seed customers to database
            modelBuilder.Entity<Customer>().HasData(customers);
        }
    }
}
