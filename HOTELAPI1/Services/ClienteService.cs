using HOTELAPI1.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HOTELAPI1.Services
{
    public class ClienteService
    {
        private readonly HotelDbContext _context;

        public ClienteService(HotelDbContext context)
        {
            _context = context;
        }

        public async Task InsertDataFromJsonAsync(string jsonData)
        {
            // Deserializar los datos JSON en una lista de clientes
            var clientes = JsonSerializer.Deserialize<List<Cliente>>(jsonData);

            // Verificar si los datos no son nulos
            if (clientes != null)
            {
                // Agregar los clientes a la base de datos
                await _context.Clientes.AddRangeAsync(clientes);

                // Guardar los cambios
                await _context.SaveChangesAsync();
            }
        }
    }
}


