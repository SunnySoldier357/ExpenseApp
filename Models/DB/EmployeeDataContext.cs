using Microsoft.EntityFrameworkCore;

namespace ExpenseApp.Models.DB
{
    public class ExpenseDBDataContext : DbContext
    {
        // Public Properties
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ExpenseForm> ExpenseForms { get; set; }
        public DbSet<ExpenseEntry> ExpenseEntries { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Location> Locations { get; set; }

        // Constructors
        public ExpenseDBDataContext(DbContextOptions<ExpenseDBDataContext> options)
            : base(options) => Database.EnsureCreated();

        // Overridden Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Approver)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // Auto generate GUIDs 
            modelBuilder.Entity<Employee>()
                .Property(e => e.Id)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<ExpenseEntry>()
                .Property(ee => ee.Id)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Receipt>()
                .Property(r => r.Id)
                .HasDefaultValueSql("NEWID()");

            // Use properties not fields as there is business logic in setter
            modelBuilder.Entity<ExpenseEntry>()
                .Property(ee => ee.Cost)
                .HasField("_cost")
                .UsePropertyAccessMode(PropertyAccessMode.Property);
        }
    }
}