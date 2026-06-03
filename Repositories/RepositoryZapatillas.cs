using Microsoft.EntityFrameworkCore;
using MvcZapatillasExamenAnf.Data;
using MvcZapatillasExamenAnf.Models;

namespace MvcZapatillasExamenAnf.Repositories
{
    public class RepositoryZapatillas
    {
        private AppDbContext context;

        public RepositoryZapatillas(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Zapatilla>> GetZapatillasAsync()
        {
            return await this.context.Zapatillas.ToListAsync();
        }

        public async Task InsertZapatillaAsync(Zapatilla zapatilla)
        {
            this.context.Zapatillas.Add(zapatilla);
            await this.context.SaveChangesAsync();
        }
    }
}
