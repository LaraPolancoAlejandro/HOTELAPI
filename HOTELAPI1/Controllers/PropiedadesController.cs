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
