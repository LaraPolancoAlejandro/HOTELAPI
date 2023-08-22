using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HOTELAPI1;
using HOTELAPI1.Models;
using HOTELAPI1.Dtos;

namespace HOTELAPI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropiedadesController : ControllerBase
    {
        private readonly HotelDbContext _context;

        public PropiedadesController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: api/Propiedades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Propiedad>>> GetPropiedades()
        {
            return await _context.Propiedades.ToListAsync();
        }

        // GET: api/Propiedades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Propiedad>> GetPropiedad(int id)
        {
            var propiedad = await _context.Propiedades.FindAsync(id);

            if (propiedad == null)
            {
                return NotFound();
            }

            return propiedad;
        }

        // PUT: api/Propiedades/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePropiedad(int id, Propiedad propiedad)
        {
            if (id != propiedad.Id)
            {
                return BadRequest();
            }

            _context.Entry(propiedad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropiedadExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Propiedades
        [HttpPost]
        public async Task<ActionResult<Propiedad>> CreatePropiedad(Propiedad propiedad)
        {
            _context.Propiedades.Add(propiedad);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPropiedad", new { id = propiedad.Id }, propiedad);
        }

        // DELETE: api/Propiedades/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Propiedad>> DeletePropiedad(int id)
        {
            var propiedad = await _context.Propiedades.FindAsync(id);
            if (propiedad == null)
            {
                return NotFound();
            }

            _context.Propiedades.Remove(propiedad);
            await _context.SaveChangesAsync();

            return propiedad;
        }

        private bool PropiedadExists(int id)
        {
            return _context.Propiedades.Any(e => e.Id == id);
        }
    

        [HttpPost("registrar-propiedad")]
        public async Task<IActionResult> RegistrarPropiedad([FromForm] PropiedadRegistroDto dto)
        {
            // Validar el modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar si el propietario está validado
            var propietario = await _context.Propietarios.FindAsync(dto.PropietarioId);
            if (propietario == null || !propietario.IsEmailConfirmed)
            {
                return BadRequest(new { Message = "La cuenta del propietario debe estar validada para registrar una propiedad." });
            }

            // Crear la propiedad
            var propiedad = new Propiedad
            {
                Nombre = dto.Nombre,
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

    }
}
