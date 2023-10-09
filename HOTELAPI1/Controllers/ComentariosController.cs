﻿using Microsoft.AspNetCore.Mvc;
using HOTELAPI1.Collections;
using HOTELAPI1.Services;
using System.Threading.Tasks;
using MongoDB.Bson;
using System.Security.Claims;
using HOTELAPI1.Abstract;

namespace HOTELAPI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentariosController : ControllerBase
    {
        private readonly ComentarioService _comentarioService;
        private readonly IPropiedadService _propiedadService; // Asegúrate de tener un servicio similar

        public ComentariosController(ComentarioService comentarioService, IPropiedadService propiedadService) // Inyecta aquí
        {
            _comentarioService = comentarioService;
            _propiedadService = propiedadService; // Asigna el servicio inyectado a la variable de la clase
        }

        [HttpPost]
        public async Task<IActionResult> CreateComentario([FromBody] Comentario comentario)
        {
            // Obtener el ClienteId del usuario logueado con Clerk
            var clienteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(clienteId))
               // return Unauthorized();

            comentario.ClienteId = clienteId;

            // Validar que el PropiedadId existe
            var propiedad = await _propiedadService.GetPropiedadById(comentario.PropiedadId);
            if (propiedad == null)
                return BadRequest("PropiedadId no válido");

            await _comentarioService.CreateComentario(comentario);

            return CreatedAtRoute("GetComentario", new { id = comentario.Id.ToString() }, comentario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComentario(string id, [FromBody] Comentario comentario)
        {
            if (comentario == null)
                return BadRequest("Comentario is required");

            var existingComentario = await _comentarioService.GetComentarioById(new ObjectId(id));
            if (existingComentario == null)
                return NotFound("Comentario not found");

            await _comentarioService.UpdateComentario(new ObjectId(id), comentario);

            return NoContent(); // Retorna un 204 No Content, que es una práctica común después de un PUT exitoso.
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComentario(string id)
        {
            var comentario = await _comentarioService.GetComentarioById(new ObjectId(id));
            if (comentario == null)
                return NotFound("Comentario not found");

            await _comentarioService.DeleteComentario(new ObjectId(id));

            return NoContent(); // Retorna un 204 No Content, que es una práctica común después de un DELETE exitoso.
        }

        [HttpGet("{id}", Name = "GetComentario")]
        public async Task<ActionResult<Comentario>> GetComentario(ObjectId id)
        {
            if (id == ObjectId.Empty)
                return BadRequest("Id is required");

            var comentario = await _comentarioService.GetComentarioById(id);
            if (comentario == null)
            {
                return NotFound();
            }
            return Ok(comentario);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comentario>>> GetComentarios()
        {
            var comentarios = await _comentarioService.GetComentarios();
            return Ok(comentarios);
        }

        [HttpGet("ByPropiedad/{propiedadId}")]
        public async Task<ActionResult<IEnumerable<Comentario>>> GetComentariosByPropiedadId(
        Guid propiedadId,
        [FromQuery] int? pageNumber = null,
        [FromQuery] int? pageSize = null)
        {
            var comentarios = await _comentarioService.GetComentariosByPropiedadId(propiedadId, pageNumber, pageSize);
            if (comentarios == null || !comentarios.Any())
            {
                return NotFound();
            }
            return Ok(comentarios);
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
            try
            {
                await _comentarioService.InsertDataFromJsonAsync(fileContent);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while importing data: {ex.Message}");
            }

            return Ok("Data inserted successfully!");
        }

    }
}
