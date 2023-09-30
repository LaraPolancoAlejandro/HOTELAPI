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
    public class ClientesController : ControllerBase
    {
        private readonly HotelDbContext _context;

        public ClientesController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            if (_context.Clientes == null)
            {
                return NotFound();
            }
            return await _context.Clientes.ToListAsync();
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(string id)
        {
            if (_context.Clientes == null)
            {
                return NotFound();
            }
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        // PUT: api/Clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(string id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest();
            }

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
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

        // POST: api/Clientes
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            if (_context.Clientes == null)
            {
                return Problem("Entity set 'HotelDbContext.Clientes' is null.");
            }
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCliente", new { id = cliente.Id }, cliente);
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(string id)
        {
            if (_context.Clientes == null)
            {
                return NotFound();
            }
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClienteExists(string id)
        {
            return (_context.Clientes?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // New commands
        // POST: api/Clientes/Register
        [HttpPost("Register")]
        public async Task<ActionResult<Cliente>> RegisterCliente([FromForm] ClienteRegisterDto clienteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cliente = new Cliente
            {
                Id = clienteDto.Id,
                Nombre = clienteDto.Nombre,
                Apellido = clienteDto.Apellido,
                Email = clienteDto.Email,
                PhoneNumber = clienteDto.PhoneNumber,
                Password = clienteDto.Password
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCliente", new { id = cliente.Id }, cliente);
        }
    }
}
