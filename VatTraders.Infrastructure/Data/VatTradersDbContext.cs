using Microsoft.EntityFrameworkCore;
using VatTraders.Domain.Entities;

namespace VatTraders.Infrastructure.Data;

public class VatTradersDbContext : DbContext
{
    public VatTradersDbContext(DbContextOptions<VatTradersDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(p => p.Name).HasMaxLength(120).IsRequired();
            entity.Property(p => p.HsnCode).HasMaxLength(8).HasDefaultValue("3808").IsRequired();
            entity.Property(p => p.UnitPrice).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(c => c.Name).HasMaxLength(120).IsRequired();
            entity.Property(c => c.Phone).HasMaxLength(20);
            entity.Property(c => c.Gstin).HasMaxLength(20);
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.Property(i => i.SubTotal).HasPrecision(18, 2);
            entity.Property(i => i.GstTotal).HasPrecision(18, 2);
            entity.Property(i => i.Cgst).HasPrecision(18, 2);
            entity.Property(i => i.Sgst).HasPrecision(18, 2);
            entity.Property(i => i.GrandTotal).HasPrecision(18, 2);
            entity.HasMany(i => i.Items).WithOne(i => i.Invoice).HasForeignKey(i => i.InvoiceId);
        });

        modelBuilder.Entity<InvoiceItem>(entity =>
        {
            entity.Property(i => i.UnitPrice).HasPrecision(18, 2);
            entity.Property(i => i.TaxRate).HasPrecision(5, 4);
            entity.Property(i => i.TaxAmount).HasPrecision(18, 2);
            entity.Property(i => i.LineTotal).HasPrecision(18, 2);
            entity.HasOne(i => i.Product).WithMany().HasForeignKey(i => i.ProductId);
        });
    }
}
