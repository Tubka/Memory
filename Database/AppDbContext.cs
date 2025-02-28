using Microsoft.EntityFrameworkCore;

namespace Memory.Database;

public class AppDbContext : DbContext
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<ActiveCompany> ActiveCompanies { get; set; }
    public DbSet<CheckboxTemplate> CheckboxTemplates { get; set; }
    public DbSet<MonthlyCheckbox> MonthlyCheckboxes { get; set; }
    public DbSet<Description> Descriptions { get; set; }
    public DbSet<MonthDescription> MonthDescriptions { get; set; }
    public DbSet<FuelDescription> FuelDescriptions { get; set; }
    public DbSet<User> Users { get; set; }

    public static void InitializeDatabase()
    {
        using (var context = new AppDbContext())
        {
            context.Database.EnsureCreated();
            Console.WriteLine("Baza danych została zainicjowana.");
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Data Source=.\mydatabase.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MonthlyCheckbox>()
            .HasOne(mc => mc.Company)
            .WithMany(c => c.MonthlyCheckboxes)
            .HasForeignKey(mc => mc.CompanyId);

        modelBuilder.Entity<MonthlyCheckbox>()
            .HasOne(mc => mc.CheckboxTemplate)
            .WithMany(ct => ct.MonthlyCheckboxes)
            .HasForeignKey(mc => mc.TemplateId);

        base.OnModelCreating(modelBuilder);
    }
}
