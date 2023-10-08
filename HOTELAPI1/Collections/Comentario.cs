using MongoDB.Bson;

namespace HOTELAPI1.Collections
{
    public class Comentario
    {
        public ObjectId Id { get; set; } // Identificador único en MongoDB
        public string ClienteId { get; set; } // Identificador del Cliente
        public string Texto { get; set; } // Texto del comentario
        public Guid PropiedadId { get; set; }
        public DateTime Fecha { get; set; } // Fecha en que se hizo el comentario
    }
}