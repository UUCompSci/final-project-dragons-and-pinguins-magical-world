using Microsoft.EntityFrameworkCore;
using Entities;
using System;//might need to console
using System.IO;//might need for path

//jimmyfigged with some stuff here
public class ZooDbContext : DbContext
{
    public DbSet<Penguin> Penguins { get; set; }
    public DbSet<Dragon> Dragons { get; set; }
    public DbSet<Enclosure> Enclosures { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string path = Path.Combine(Environment.CurrentDirectory, "Zoo.db");
        string connection = $"Filename={path}";
        
        Console.WriteLine($"Connection: {connection}");
        optionsBuilder.UseSqlite(connection);
    }
}

