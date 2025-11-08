using Microsoft.EntityFrameworkCore;

namespace DDDExample.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Ejemplo de entidad del dominio (cuando la creemos):
        // public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aquí van las configuraciones con Fluent API
            // modelBuilder.Entity<Product>().ToTable("Products");
        }
    }
}
