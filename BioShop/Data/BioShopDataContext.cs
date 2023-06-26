using BioShop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BioShop.Data
{
    public class BioShopDataContext : DbContext
    {
        public BioShopDataContext(DbContextOptions<BioShopDataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //product one to many

            modelBuilder.Entity<Product>()
                .HasMany(r => r.Recipes)
                .WithOne(p => p.CurrentProduct)
                .HasForeignKey(k => k.CurrentProductId)
                .OnDelete(DeleteBehavior.Cascade);

            //clientProducts many to many

            modelBuilder.Entity<ClientProduct>()
                .HasOne(c => c.Client)
                .WithMany(p => p.Clients_Products)
                .HasForeignKey(k => k.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClientProduct>()
                .HasOne(p => p.Product)
                .WithMany(c => c.Clients_Products)
                .HasForeignKey(k => k.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ClientProduct> ClientProducts { get; set; }
    }
}