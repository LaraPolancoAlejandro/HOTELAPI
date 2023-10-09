using HOTELAPI1.Models;
using Humanizer;
using System.Text.Json;

namespace HOTELAPI1.Services
{
    public class ReservacionService
    {
        private readonly HotelDbContext _context;

        public ReservacionService(HotelDbContext context)
        {
            _context = context;
        }
        public async Task InsertDataFromJsonAsync(string jsonData)
        {
            try
            {
                var reservaciones = JsonSerializer.Deserialize<List<Reservacion>>(jsonData);
                if (reservaciones == null || reservaciones.Count == 0)
                {
                    throw new Exception("No data to import");
                }

                await _context.Reservaciones.AddRangeAsync(reservaciones);
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
        public async Task<bool> UpdateReservacionAsync(Guid id, Reservacion updatedReservacion)
        {
            var reservacion = await _context.Reservaciones.FindAsync(id);
            if (reservacion == null)
            {
                return false;
            }

            reservacion.FechaInicio = updatedReservacion.FechaInicio;
            reservacion.FechaFin = updatedReservacion.FechaFin;
            reservacion.Estado = updatedReservacion.Estado;
            reservacion.Total = updatedReservacion.Total;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task UpdateReservacionStatusAsync()
        {
            int i = 1;
            var reservaciones = _context.Reservaciones.ToList();

            foreach (var reservacion in reservaciones)
            {
                //Console.WriteLine("R" + i);
                var now = DateTime.Now;

                // Verificar si la propiedad asociada a la reservación aún existe
                var propiedad = await _context.Propiedades.FindAsync(reservacion.PropiedadId);
                if (propiedad == null)
                {
                    reservacion.Estado = "Eliminado";
                }
                else if (reservacion.Estado != "Cancelada" && reservacion.Estado != "Eliminado")
                {
                    if (now < reservacion.FechaInicio)
                    {
                        reservacion.Estado = "Prepara tus maletas";
                    }
                    else if (now >= reservacion.FechaInicio && now <= reservacion.FechaFin)
                    {
                        reservacion.Estado = "Disfruta el viaje";
                    }
                    else if (now > reservacion.FechaFin)
                    {
                        reservacion.Estado = "Expirada";
                    }
                }
                i++;
            }
            await _context.SaveChangesAsync();
        }
    }
}



