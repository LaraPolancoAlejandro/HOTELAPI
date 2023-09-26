namespace HOTELAPI1.Models
{
    public class Reservacion
    {
        public Guid Id { get; set; }
        public int ClienteId { get; set; }
        public int PropiedadId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }
        public decimal Total { get; set; }
    }


}
