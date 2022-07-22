using Microsoft.EntityFrameworkCore;
using SQL_StoreProcedure.Controllers;

namespace SQL_StoreProcedure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestVM>()
                .HasKey(x => x.IdTest);
        }

        public DbSet<TestVM> Test { get; set; }
    }
}