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
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [RegularExpression(@"^(?=.*[A-Z]).+$", ErrorMessage = "La contraseña debe contener al menos una letra mayúscula.")]
        public string Password { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
