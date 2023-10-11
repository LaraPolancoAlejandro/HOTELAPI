using HOTELAPI1.Models;

namespace HOTELAPI1.Abstract
{
    public interface IClienteService
    {
        Task<Cliente> GetClienteById(string clienteId);
        Task InsertDataFromJsonAsync(string jsonData);
        
    }
}
