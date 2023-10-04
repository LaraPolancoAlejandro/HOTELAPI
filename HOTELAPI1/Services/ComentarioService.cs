using MongoDB.Driver;
using HOTELAPI1.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

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


    }
}
