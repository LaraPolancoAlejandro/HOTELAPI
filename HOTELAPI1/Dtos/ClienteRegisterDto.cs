using System.ComponentModel.DataAnnotations;

namespace HOTELAPI1.Dtos
{
    public class ClienteRegisterDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime FechaRegistro { get; set; }
    }
}
