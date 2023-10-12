using HOTELAPI1.Dtos;
using HOTELAPI1.Models;
using HOTELAPI1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using HOTELAPI1.Services;
using Microsoft.EntityFrameworkCore.Metadata;
using Humanizer;

namespace HOTELAPI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ReservacionesController : ControllerBase
    {
        private readonly HotelDbContext _context;
        private readonly ReservacionService _service;
        public ReservacionesController(HotelDbContext context, ReservacionService service)
        {
            _service = service;
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

            // Verificar si ya existe una reservación que se superponga con las fechas especificadas
            var overlappingReservacion = await _context.Reservaciones
                .Where(r => r.PropiedadId == reservacionDto.PropiedadId)
                .Where(r =>
                    (reservacionDto.FechaInicio >= r.FechaInicio && reservacionDto.FechaInicio <= r.FechaFin) ||
                    (reservacionDto.FechaFin >= r.FechaInicio && reservacionDto.FechaFin <= r.FechaFin) ||
                    (reservacionDto.FechaInicio <= r.FechaInicio && reservacionDto.FechaFin >= r.FechaFin))
                .FirstOrDefaultAsync();

            if (overlappingReservacion != null)
            {
                return BadRequest("Ya existe una reservación que se superpone con las fechas especificadas.");
            }
            var propiedad = await _context.Propiedades.FindAsync(reservacionDto.PropiedadId);
            if (propiedad == null)
            {
                return NotFound("Propiedad no encontrada");
            }

            var duracionEstancia = (reservacionDto.FechaFin - reservacionDto.FechaInicio).Days;

            // Calcular el total
            var total = duracionEstancia * propiedad.PrecioPorNoche;

            // Crear la reservación
            var reservacion = new Reservacion
            {
                ClienteId = reservacionDto.ClienteId,
                PropiedadId = reservacionDto.PropiedadId,
                FechaInicio = reservacionDto.FechaInicio,
                FechaFin = reservacionDto.FechaFin,
                Estado = reservacionDto.Estado,
                Total = total
            };


            _context.Reservaciones.Add(reservacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReservacion", new { id = reservacion.Id }, reservacion);
        }


        // DELETE: api/Reservaciones/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Reservacion>> DeleteReservacion(Guid id)
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

        // GET: api/Reservaciones/Usuario/{usuarioId}
        [HttpGet("Usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<Reservacion>>> GetReservacionesByUsuarioId(string usuarioId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            // Validar si el usuario existe
            var usuario = await _context.Clientes.FindAsync(usuarioId);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado");
            }

            // Obtener las reservaciones del usuario con paginación
            var reservaciones = await _context.Reservaciones
                .Where(r => r.ClienteId == usuarioId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Obtener el total de reservaciones para el usuario
            var totalReservaciones = await _context.Reservaciones.CountAsync(r => r.ClienteId == usuarioId);

            // Configurar la paginación en los headers de la respuesta
            Response.Headers.Add("X-Total-Count", totalReservaciones.ToString());
            Response.Headers.Add("X-Page-Number", pageNumber.ToString());
            Response.Headers.Add("X-Page-Size", pageSize.ToString());

            // Retornar las reservaciones
            return Ok(reservaciones);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservacion(Guid id, Reservacion updatedReservacion)
        {
            try
            {
                if (updatedReservacion.FechaInicio >= updatedReservacion.FechaFin)
                {
                    return BadRequest("La fecha de inicio debe ser anterior a la fecha final.");
                }

                // Obtener la propiedad por su ID
                var propiedad = await _context.Propiedades.FindAsync(updatedReservacion.PropiedadId);
                if (propiedad == null)
                {
                    // Cambiar el estado de la reservación a "Eliminado"
                    var reservacion = await _context.Reservaciones.FindAsync(id);
                    if (reservacion != null)
                    {
                        reservacion.Estado = "Eliminado";
                        await _context.SaveChangesAsync();
                    }
                    return Ok(new { Message = "La propiedad no existe o ha sido eliminada. La reservación ha sido marcada como 'Eliminado'." });
                }

                // Calcular la duración de la estancia con las nuevas fechas
                var duracionEstancia = (updatedReservacion.FechaFin - updatedReservacion.FechaInicio).Days;

                // Calcular el nuevo total con las nuevas fechas
                var total = duracionEstancia * propiedad.PrecioPorNoche;

                // Actualizar el total de la reservación
                updatedReservacion.Total = total;

                var result = await _service.UpdateReservacionAsync(id, updatedReservacion);
                if (!result)
                {
                    return NotFound(new { Message = "Reservación no encontrada" });
                }
                return Ok(new { Message = "Reservación actualizada con éxito" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }



        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            // Verificar si el archivo es nulo
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is not selected");
            }

            // Leer el archivo y convertirlo a string
            using var reader = new StreamReader(file.OpenReadStream());
            var fileContent = await reader.ReadToEndAsync();

            // Insertar los datos en la base de datos
            await _service.InsertDataFromJsonAsync(fileContent);

            return Ok("Data inserted successfully!");
        }

    }
}
    

