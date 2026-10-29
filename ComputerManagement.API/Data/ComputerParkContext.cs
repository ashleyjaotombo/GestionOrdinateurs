using ComputerManagement.Shared;
using Microsoft.EntityFrameworkCore;

namespace ComputerManagement.API;

public class ComputerParkContext : DbContext
{
    public DbSet<Computer> Computers { get; set; }
    public DbSet<Repair> Repairs {get; set;}

    public DbSet<User> Users { get; set; }

    public ComputerParkContext(DbContextOptions <ComputerParkContext> options) : base (options){}

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=parcinfo.db");
        
    }*/

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Computer>()
        .OwnsOne(o => o.Characteristic);

    modelBuilder.Entity<Computer>()
        .HasMany(o => o.Repairs)
        .WithOne(r => r.Computer);
}


}