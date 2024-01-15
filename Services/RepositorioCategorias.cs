using ControlPresupuesto.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ControlPresupuesto.Services
{
    public interface IRepositorioCategorias
    {
        Task Actualizar(Categoria categoria);
        Task Borrar(int id);
        Task Crear(Categoria categoria);
        Task<Categoria> GeTById(int id, int usuarioId);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId, TipoOperacion tipoOperacionId);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId);
    }
    public class RepositorioCategorias : IRepositorioCategorias
    {
        private readonly string connectionStrings;

        public RepositorioCategorias(IConfiguration configuration)
        {
            connectionStrings = configuration.GetConnectionString("DefaultConnection");

            
        }

        public async Task Crear (Categoria categoria)
        {
            using SqlConnection connection = new(connectionStrings);
            int id = await connection.QuerySingleAsync<int>(@"
                                        INSERT INTO Categorias (Nombre, TipoOperacionId, UsuarioId)
                                        Values (@Nombre, @TipoOperacionId, @UsuarioId);

                                        SELECT SCOPE_IDENTITY();
                                        ", categoria);

            categoria.Id = id;

        }

        public async Task<IEnumerable<Categoria>> Obtener (int usuarioId)
        {
            using SqlConnection connection = new(connectionStrings);
            return await connection.QueryAsync<Categoria>("SELECT * FROM Categorias WHERE UsuarioId = @usuarioId", new { usuarioId });

        }

        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId, TipoOperacion tipoOperacionId)
        {
            using SqlConnection connection = new(connectionStrings);
            return await connection.QueryAsync<Categoria>(
                @"SELECT * 
            FROM Categorias 
            WHERE UsuarioId = @usuarioId AND TipoOperacionId = @tipoOperacionId",
                new { usuarioId, tipoOperacionId });
        }

        public async Task<Categoria> GeTById(int id, int usuarioId)
        {
            using SqlConnection connection = new(connectionStrings);
            return await connection.QueryFirstOrDefaultAsync<Categoria>(
                        @"Select * FROM Categorias WHERE Id = @Id AND UsuarioId = @UsuarioId",
                        new { id, usuarioId });
        }

        public async Task Actualizar(Categoria categoria)
        {
            using SqlConnection connection = new(connectionStrings);
            await connection.ExecuteAsync(@"UPDATE Categorias 
                    SET Nombre = @Nombre, TipoOperacionId = @TipoOperacionID
                    WHERE Id = @Id", categoria);
        }

        public async Task Borrar(int id)
        {
            using SqlConnection connection = new(connectionStrings);
            await connection.ExecuteAsync("DELETE Categorias WHERE Id = @Id", new { id });
        }

    }
}
