namespace HOTELAPI1.Models
{
    public class Reservacion
    {
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public Guid PropiedadId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }
        public decimal Total { get; set; }
    }


}
