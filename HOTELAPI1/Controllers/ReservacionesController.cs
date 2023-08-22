using HOTELAPI1.Dtos;
using HOTELAPI1.Models;
using HOTELAPI1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;


namespace HOTELAPI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ReservacionesController : ControllerBase
    {
        private readonly HotelDbContext _context;

        public ReservacionesController(HotelDbContext context)
        {
            _context = context;
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearReservacion([FromForm] ReservacionDto dto)
        {
            // Validar el modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Validar que la fecha de inicio sea anterior a la fecha final
            if (dto.FechaInicio >= dto.FechaFin)
            {
                return BadRequest("La fecha de inicio debe ser anterior a la fecha final.");
            }
            // Obtener la propiedad por su ID
            var propiedad = await _context.Propiedades.FindAsync(dto.PropiedadId);
            if (propiedad == null)
            {
                return NotFound("Propiedad no encontrada");
            }

            // Calcular la duración de la estancia
            var duracionEstancia = (dto.FechaFin - dto.FechaInicio).Days;

            // Calcular el total
            var total = duracionEstancia * propiedad.PrecioPorNoche;

            // Crear la reservación
            var reservacion = new Reservacion
            {
                ClienteId = dto.ClienteId,
                PropiedadId = dto.PropiedadId,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                Estado = dto.Estado,
                Total = total
            };

            // Guardar la reservación en la base de datos
            _context.Reservaciones.Add(reservacion);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Reservación creada con éxito.", ReservacionId = reservacion.Id });
        }
    }
}
    

