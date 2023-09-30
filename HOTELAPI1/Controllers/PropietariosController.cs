using HOTELAPI1.Dtos;
using HOTELAPI1.Models;
using HOTELAPI1;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using HOTELAPI1.Services;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class PropietariosController : ControllerBase
{
    private readonly HotelDbContext _context;
    private readonly EmailService _emailService;
    public PropietariosController(HotelDbContext context, EmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }
    // GET: api/Propietarios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Propietario>>> GetPropietarios()
    {
        return await _context.Propietarios.ToListAsync();
    }

    // GET: api/Propietarios/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Propietario>> GetPropietario(int id)
    {
        var propietario = await _context.Propietarios.FindAsync(id);

        if (propietario == null)
        {
            return NotFound();
        }

        return propietario;
    }

    // PUT: api/Propietarios/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePropietario(string id, Propietario propietario)
    {
        if (id != propietario.Id)
        {
            return BadRequest();
        }

        _context.Entry(propietario).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PropietarioExists(id))
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

    // POST: api/Propietarios
    [HttpPost]
    public async Task<ActionResult<Propietario>> CreatePropietario(Propietario propietario)
    {
        _context.Propietarios.Add(propietario);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetPropietario", new { id = propietario.Id }, propietario);
    }

    // DELETE: api/Propietarios/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Propietario>> DeletePropietario(string id)
    {
        var propietario = await _context.Propietarios.FindAsync(id);
        if (propietario == null)
        {
            return NotFound();
        }

        _context.Propietarios.Remove(propietario);
        await _context.SaveChangesAsync();

        return propietario;
    }

    private bool PropietarioExists(string id)
    {
        return _context.Propietarios.Any(e => e.Id == id);
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] PropietarioRegistroDto dto)
    {
        // Validar el modelo
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Crear el propietario
        var propietario = new Propietario
        {
            Id= dto.Id,
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Email = dto.Email,
            Password = dto.Password,  // Deberías hashear la contraseña antes de almacenarla
            PhoneNumber = dto.PhoneNumber,
            IsEmailConfirmed = false
        };

        _context.Propietarios.Add(propietario);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Registro exitoso, ya puedes publicar tus propiedades!" });
    }
}

