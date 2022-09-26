using Microsoft.EntityFrameworkCore;

namespace ModelsDb
{
    public class AppContextDb : DbContext
    {
        public DbSet<ClientDb> Clients { get; set; }
        public DbSet<EmployeeDb> Employees { get; set; }
        public DbSet<AccountDb> Accounts { get; set; }
        
        public AppContextDb() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost; Port = 5433; Database = bank_system; Username = postgres; Password = 0000");
        }
    }
}