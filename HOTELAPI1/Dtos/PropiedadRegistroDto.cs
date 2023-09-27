using System.ComponentModel.DataAnnotations;

namespace HOTELAPI1.Dtos
{
    public class PropiedadRegistroDto
    {
        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo Imagen es obligatorio.")]
        public string Imagen { get; set; }

        [Required(ErrorMessage = "El campo Descripción es obligatorio.")]
        [MinLength(100, ErrorMessage = "El campo Descripción debe tener al menos 100 caracteres")]
     
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo Dirección es obligatorio.")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El campo Tipo es obligatorio.")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "El campo PropietarioId es obligatorio.")]
        public Guid PropietarioId { get; set; }

        [Required(ErrorMessage = "El campo PrecioPorNoche es obligatorio.")]
        public decimal PrecioPorNoche { get; set; }

        [Required(ErrorMessage = "El campo NumeroDeHabitaciones es obligatorio.")]
        public int NumeroDeHabitaciones { get; set; }
    }

}
