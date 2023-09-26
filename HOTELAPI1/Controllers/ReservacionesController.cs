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
        // GET: api/Reservaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservacion>>> GetReservaciones()
        {
            return await _context.Reservaciones.ToListAsync();
        }

        // GET: api/Reservaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reservacion>> GetReservacion(int id)
        {
            var reservacion = await _context.Reservaciones.FindAsync(id);

            if (reservacion == null)
            {
                return NotFound();
            }

            return reservacion;
        }

       

        // POST: api/Reservaciones
        [HttpPost]
        public async Task<ActionResult<Reservacion>> CreateReservacion(ReservacionDto reservacionDto)
        {
            if (reservacionDto.FechaInicio >= reservacionDto.FechaFin)
            {
                return BadRequest("La fecha de inicio debe ser anterior a la fecha de fin.");
            }

            var reservacion = new Reservacion
            {
                ClienteId = reservacionDto.ClienteId,
                PropiedadId = reservacionDto.PropiedadId,
                FechaInicio = reservacionDto.FechaInicio,
                FechaFin = reservacionDto.FechaFin,
                Estado = reservacionDto.Estado
            };

            _context.Reservaciones.Add(reservacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReservacion", new { id = reservacion.Id }, reservacion);
        }

        // DELETE: api/Reservaciones/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Reservacion>> DeleteReservacion(int id)
        {
            var reservacion = await _context.Reservaciones.FindAsync(id);
            if (reservacion == null)
            {
                return NotFound();
            }

            _context.Reservaciones.Remove(reservacion);
            await _context.SaveChangesAsync();

            return reservacion;
        }

        private bool ReservacionExists(Guid id)
        {
            return _context.Reservaciones.Any(e => e.Id == id);
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
    

