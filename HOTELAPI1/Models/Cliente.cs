using System.ComponentModel.DataAnnotations;

namespace HOTELAPI1.Models
{
    public class Cliente
    {
        
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }


}
