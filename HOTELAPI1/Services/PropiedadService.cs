using System.Threading.Tasks;
using HOTELAPI1.Models; // Asegúrate de usar tu espacio de nombres correcto
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using HOTELAPI1.Abstract;

namespace HOTELAPI1.Services
{
    public class PropiedadService : IPropiedadService
    {
        private readonly HotelDbContext _context;

        public PropiedadService(HotelDbContext context)
        {
            _context = context;
        }

    public async Task<Propiedad> GetPropiedadById(Guid propiedadId)
    {
        // Reemplaza "Propiedad" con tu modelo de entidad real y ajusta la lógica según tus necesidades
        return await _context.Propiedades.FirstOrDefaultAsync(p => p.Id == propiedadId);
    }
    public async Task InsertDataFromJsonAsync(string jsonData)
        {
            try
            {
                var propiedades = JsonSerializer.Deserialize<List<Propiedad>>(jsonData);
                if (propiedades == null || propiedades.Count == 0)
                {
                    throw new Exception("No data to import");
                }

                await _context.Propiedades.AddRangeAsync(propiedades);
                await _context.SaveChangesAsync();
            }
            catch (JsonException ex)
            {
                throw new Exception($"Invalid JSON format: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Database operation failed: {ex.Message}", ex);
            }
        }
    }
}
