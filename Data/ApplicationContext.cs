using Microsoft.EntityFrameworkCore;
using MiniProjekt.Models;

namespace MiniProjekt.Data
{
        public class ApplicationContext : DbContext
        {
            public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

            public DbSet<Person> Persons { get; set; }
            public DbSet<Interest> Interests { get; set; }
            public DbSet<InterestLink> InterestLinks { get; set; }
        }
    }