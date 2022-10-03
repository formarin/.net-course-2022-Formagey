using Microsoft.EntityFrameworkCore;

namespace ModelsDb
{
    public class ApplicationContextDb : DbContext
    {
        public DbSet<ClientDb> Clients { get; set; }
        public DbSet<EmployeeDb> Employees { get; set; }
        public DbSet<AccountDb> Accounts { get; set; }
        //public DbSet<CurrencyDb> Currencies { get; set; }

        public ApplicationContextDb() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost; Port = 5433; Database = bank_system; Username = postgres; Password = 0000");
        }
    }
}