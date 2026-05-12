using Microsoft.EntityFrameworkCore;
using romashka_core;
using System.Reflection.Emit;

namespace oma_structure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Document> Documents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\MSSQLLocalDB;Database=DocumentsDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContractDocument>();
            modelBuilder.Entity<ApplicationDocument>();
            modelBuilder.Entity<MemoDocument>();

            base.OnModelCreating(modelBuilder);
        }
    }
}