using HOTELAPI1.Dtos;
using HOTELAPI1.Models;
using HOTELAPI1;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using HOTELAPI1.Services;

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
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Email = dto.Email,
            Password = dto.Password,  // Deberías hashear la contraseña antes de almacenarla
            PhoneNumber = dto.PhoneNumber,
            IsEmailConfirmed = false
        };

        _context.Propietarios.Add(propietario);
        await _context.SaveChangesAsync();

        // Genera el enlace de confirmación (esto es solo un ejemplo, deberías generar un enlace seguro)
        string confirmationLink = $"https://localhost:7043/api/Propietarios/confirm-email?propietarioId={propietario.Id}";

        // Envía el correo electrónico de confirmación
        await _emailService.SendConfirmationEmailAsync(propietario.Email, confirmationLink);

        return Ok(new { Message = "Registro exitoso, por favor confirma tu correo electrónico." });
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(int propietarioId)
    {
        var propietario = await _context.Propietarios.FindAsync(propietarioId);
        if (propietario == null)
        {
            return NotFound();
        }

        propietario.IsEmailConfirmed = true;
        await _context.SaveChangesAsync();

        var htmlContent = "<html><body><h1>Tu email fue confirmado con exito, ya puedes publicar en la aplicación.</h1></body></html>";
        return Content(htmlContent, "text/html");
    }
}

