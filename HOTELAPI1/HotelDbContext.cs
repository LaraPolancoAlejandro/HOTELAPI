using Microsoft.EntityFrameworkCore;
using HOTELAPI1.Models;
using System.Reflection.Emit;
using HOTELAPI1;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HOTELAPI1
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options)
            : base(options)
        {
        }
        public class HotelDbContextFactory : IDesignTimeDbContextFactory<HotelDbContext>
        {
            public HotelDbContext CreateDbContext(string[] args)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var builder = new DbContextOptionsBuilder<HotelDbContext>();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                builder.UseSqlServer(connectionString);

                return new HotelDbContext(builder.Options);
            }
        }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Propiedad> Propiedades { get; set; }
        public DbSet<Propietario> Propietarios { get; set; }
        public DbSet<Reservacion> Reservaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}

