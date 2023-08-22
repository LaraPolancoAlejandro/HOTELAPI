using System.ComponentModel.DataAnnotations;

namespace HOTELAPI1.Dtos
{
    public class ReservacionDto
    {
        [Required]
        public int ClienteId { get; set; }
        [Required]
        public int PropiedadId { get; set; }
        [Required]
        public DateTime FechaInicio { get; set; }
        [Required]
        public DateTime FechaFin { get; set; }
        [Required]
        public string Estado { get; set; } //Activa, cancelada, expirada
    }

}
