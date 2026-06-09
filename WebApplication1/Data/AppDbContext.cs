using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductOrder> ProductOrders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("Client");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsRequired();
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("Status");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsRequired();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.Price)
                .HasColumnType("numeric(10,2)")
                .IsRequired();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.Property(e => e.FulfilledAt)
                .IsRequired(false);

            entity.HasOne(e => e.Client)
                .WithMany(c => c.Orders)
                .HasForeignKey(e => e.IdClient);

            entity.HasOne(e => e.Status)
                .WithMany(s => s.Orders)
                .HasForeignKey(e => e.IdStatus);
        });

        modelBuilder.Entity<ProductOrder>(entity =>
        {
            entity.ToTable("Product_Order");

            entity.HasKey(e => new { e.IdProduct, e.IdOrder });

            entity.Property(e => e.Amount)
                .IsRequired();

            entity.HasOne(e => e.Product)
                .WithMany(p => p.ProductOrders)
                .HasForeignKey(e => e.IdProduct);

            entity.HasOne(e => e.Order)
                .WithMany(o => o.ProductOrders)
                .HasForeignKey(e => e.IdOrder);
        });

        modelBuilder.Entity<Client>().HasData(
            new Client { Id = 1, FirstName = "John", LastName = "Doe" }
        );

        modelBuilder.Entity<Status>().HasData(
            new Status { Id = 1, Name = "Ongoing" },
            new Status { Id = 2, Name = "Completed" },
            new Status { Id = 3, Name = "Cancelled" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Bananas", Price = 5.55m },
            new Product { Id = 2, Name = "Orange", Price = 12.37m }
        );

        modelBuilder.Entity<Order>().HasData(
            new Order
            {
                Id = 1,
                CreatedAt = new DateTime(2025, 5, 2),
                FulfilledAt = null,
                IdClient = 1,
                IdStatus = 1
            }
        );

        modelBuilder.Entity<ProductOrder>().HasData(
            new ProductOrder { IdProduct = 1, IdOrder = 1, Amount = 2 },
            new ProductOrder { IdProduct = 2, IdOrder = 1, Amount = 1 }
        );
    }
}