using HOTELAPI1.Models;

namespace HOTELAPI1.Abstract
{
    public interface IPropiedadService
    {
        Task<Propiedad> GetPropiedadById(Guid propiedadId);
        Task InsertDataFromJsonAsync(string jsonData);
        // Otros métodos que tu servicio implementa...
    }

}
