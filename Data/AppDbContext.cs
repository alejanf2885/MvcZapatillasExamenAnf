using Microsoft.EntityFrameworkCore;
using MvcZapatillasExamenAnf.Models;

namespace MvcZapatillasExamenAnf.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Zapatilla> Zapatillas { get; set; }
    }
}
