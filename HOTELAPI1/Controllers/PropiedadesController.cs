using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HOTELAPI1.Models;
using HOTELAPI1.Dtos;
using HOTELAPI1.Services;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HOTELAPI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropiedadesController : ControllerBase
    {
        private readonly HotelDbContext _context;
        private readonly PropiedadService _service;
        public PropiedadesController(HotelDbContext context, PropiedadService service)
        {
            _service = service;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ListarPropiedades(
        [FromQuery] int? pageNumber = null,
        [FromQuery] int? pageSize = null,
        [FromQuery] string? nombre = null,
        [FromQuery] string? descripcion = null,
        [FromQuery] string? direccion = null,
        [FromQuery] string? tipo = null,
        [FromQuery] decimal? precioPorNocheMin = null,
        [FromQuery] decimal? precioPorNocheMax = null,
        [FromQuery] int? numeroDeHabitacionesMin = null,
        [FromQuery] int? numeroDeHabitacionesMax = null)
        {
            var query = _context.Propiedades.AsQueryable();

            // Filtrado
            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(p => p.Nombre.Contains(nombre));
            }
            if (!string.IsNullOrEmpty(descripcion))
            {
                query = query.Where(p => p.Descripcion.Contains(descripcion));
            }
            if (!string.IsNullOrEmpty(direccion))
            {
                query = query.Where(p => p.Direccion.Contains(direccion));
            }
            if (!string.IsNullOrEmpty(tipo))
            {
                query = query.Where(p => p.Tipo == tipo);
            }
            if (precioPorNocheMin.HasValue)
            {
                query = query.Where(p => p.PrecioPorNoche >= precioPorNocheMin.Value);
            }
            if (precioPorNocheMax.HasValue)
            {
                query = query.Where(p => p.PrecioPorNoche <= precioPorNocheMax.Value);
            }
            if (numeroDeHabitacionesMin.HasValue)
            {
                query = query.Where(p => p.NumeroDeHabitaciones >= numeroDeHabitacionesMin.Value);
            }
            if (numeroDeHabitacionesMax.HasValue)
            {
                query = query.Where(p => p.NumeroDeHabitaciones <= numeroDeHabitacionesMax.Value);
            }

            // Paginación
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                query = query
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }

            var propiedades = await query.ToListAsync();

            if (propiedades == null || propiedades.Count == 0)
            {
                return NotFound();
            }

            return Ok(propiedades);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPropiedadPorId(Guid id)
        {
            try
            {
                var propiedad = await _context.Propiedades.FindAsync(id);

                if (propiedad == null)
                {
                    return NotFound();
                }

                return Ok(propiedad);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener la propiedad.");
            }
        }


        [HttpPost("registrar-propiedad")]
        public async Task<IActionResult> RegistrarPropiedad([FromForm] PropiedadRegistroDto dto)
        {
            // Validar el modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //// Verificar si el propietario está validado
            //var propietario = await _context.Propietarios.FindAsync(dto.PropietarioId);
            //if (propietario == null || !propietario.IsEmailConfirmed)
            //{
            //    return BadRequest(new { Message = "La cuenta del propietario debe estar validada para registrar una propiedad." });
            //}

            // Crear la propiedad
            var propiedad = new Propiedad
            {
                Nombre = dto.Nombre,
                Imagen = dto.Imagen,
                Descripcion = dto.Descripcion,
                Direccion = dto.Direccion,
                Tipo = dto.Tipo,
                PropietarioId = dto.PropietarioId,
                PrecioPorNoche = dto.PrecioPorNoche,
                NumeroDeHabitaciones = dto.NumeroDeHabitaciones
            };

            _context.Propiedades.Add(propiedad);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Propiedad registrada con éxito." });
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

        [HttpGet("tipos")]
        public async Task<ActionResult<IEnumerable<string>>> GetPropertyTypes()
        {
            var tipos = await _context.Propiedades
                                      .Select(p => p.Tipo)
                                      .Distinct()
                                      .ToListAsync();
            return Ok(tipos);
        }
    }
}
