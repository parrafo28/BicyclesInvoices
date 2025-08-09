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
    
    // DbSet para Invoices - representa la tabla de facturas en la base de datos
    public DbSet<Invoice> Invoices { get; set; }
    
    // DbSet para Employees - necesario para los dropdowns al crear facturas
    public DbSet<Employee> Employees { get; set; }
    
    // DbSet para InvoiceDetails - detalles de cada factura
    public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
    
    // DbSet para ProductOrService - productos/servicios que se pueden facturar
    public DbSet<ProductOrService> ProductsOrServices { get; set; }
}
