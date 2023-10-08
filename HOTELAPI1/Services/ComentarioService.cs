using MongoDB.Driver;
using HOTELAPI1.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using HOTELAPI1.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using HOTELAPI1.Collections;
using HOTELAPI1.Services;
using System.Threading.Tasks;
using MongoDB.Bson;
using System.Security.Claims;

namespace HOTELAPI1.Services
{
    public class ComentarioService
    {
        private readonly IMongoCollection<Comentario> _comentarios;

        public ComentarioService(IMongoClient client)
        {
            var database = client.GetDatabase("NombreDeTuBaseDeDatos");
            _comentarios = database.GetCollection<Comentario>("Comentarios");
        }

        public async Task CreateComentario(Comentario comentario)
        {
            await _comentarios.InsertOneAsync(comentario);
        }

        public async Task UpdateComentario(ObjectId id, Comentario comentario)
        {
            await _comentarios.ReplaceOneAsync(com => com.Id == id, comentario);
        }

        public async Task DeleteComentario(ObjectId id)
        {
            await _comentarios.DeleteOneAsync(com => com.Id == id);
        }
        public async Task<Comentario> GetComentarioById(ObjectId id)
        {
            return await _comentarios.Find(comentario => comentario.Id == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Comentario>> GetComentarios()
        {
            return await _comentarios.Find(_ => true).ToListAsync();
        }
        public async Task<IEnumerable<Comentario>> GetComentariosByPropiedadId(Guid propiedadId, int? pageNumber = null, int? pageSize = null)
        {
            var filter = Builders<Comentario>.Filter.Eq(c => c.PropiedadId, propiedadId);
            var query = _comentarios.Find(filter).ToEnumerable(); // Asumiendo que _context.Comentarios es tu IMongoCollection<Comentario>

            // Paginación
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                query = query
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }

            return query.ToList();
        }

        public async Task InsertDataFromJsonAsync(string jsonData)
        {
            // Deserializar JSON a objetos Comentario
            var comentarios = JsonSerializer.Deserialize<IEnumerable<Comentario>>(jsonData);

            // Insertar datos en MongoDB
            await _comentarios.InsertManyAsync(comentarios);
        }

    }
}
