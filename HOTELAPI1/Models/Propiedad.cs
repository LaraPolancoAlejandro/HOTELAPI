namespace HOTELAPI1.Models
{
    public class Propiedad
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public string Tipo { get; set; }
        public Guid PropietarioId { get; set; }
        public decimal PrecioPorNoche { get; set; }
        public int NumeroDeHabitaciones { get; set; }
    }


}
