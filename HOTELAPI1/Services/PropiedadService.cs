using System.Threading.Tasks;
using HOTELAPI1.Models; // Asegúrate de usar tu espacio de nombres correcto
using Microsoft.EntityFrameworkCore;

namespace HOTELAPI1.Services
{
    public class PropiedadService
    {
        private readonly HotelDbContext _context; // Reemplaza YourDbContext con tu contexto de base de datos real

        public PropiedadService(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<Propiedad> GetPropiedadById(Guid propiedadId)
        {
            // Reemplaza "Propiedad" con tu modelo de entidad real y ajusta la lógica según tus necesidades
            return await _context.Propiedades.FirstOrDefaultAsync(p => p.Id == propiedadId);
        }
    }
}
