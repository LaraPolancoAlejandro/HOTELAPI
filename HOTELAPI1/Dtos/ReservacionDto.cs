using System.ComponentModel.DataAnnotations;

namespace HOTELAPI1.Dtos
{
    public class ReservacionDto
    {
        [Required]
        public string ClienteId { get; set; }
        [Required]
        public Guid PropiedadId { get; set; }
        [Required]
        public DateTime FechaInicio { get; set; }
        [Required]
        public DateTime FechaFin { get; set; }
        [Required]
        public string Estado { get; set; } //Activa, cancelada, expirada
    }

}
