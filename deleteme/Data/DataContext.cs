using deleteme.Models.Entities;
using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public DbSet<Bicycle> Bicycles { get; set; }
    public DbSet<Client> Clients { get; set; }
}
