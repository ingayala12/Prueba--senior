using Microsoft.EntityFrameworkCore;
using PruebaCliente.Modelos;

namespace PruebaCliente.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
    }
}
