using ControlPresupuesto.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ControlPresupuesto.Services
{
    public interface IRepositorioTiposCuentas
    {
     
        Task Borrar(int id);
        Task Crear(TipoCuenta tipoCuenta);
        Task<bool> Existe(string nombre, int usuarioid);
        Task<IEnumerable<TipoCuenta>> Get(int usuarioid);
        Task<TipoCuenta> GetById(int id, int usuarioId);
        Task Ordenar(IEnumerable<TipoCuenta> tipoCuentasOrdenados);
        Task Update(TipoCuenta tipoCuenta);
    }
    public class RepositorioTipoCuentas : IRepositorioTiposCuentas
    {
        private readonly string connectionStrings;
        public RepositorioTipoCuentas(IConfiguration configuration)
        {
            connectionStrings = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(TipoCuenta tipoCuenta)
        {
            using SqlConnection connection = new(connectionStrings);
            int id = await connection.QuerySingleAsync<int>("TiposCuentas_Insertar", new { usuarioId = tipoCuenta.UsuarioId, nombre = tipoCuenta.Nombre }, commandType: System.Data.CommandType.StoredProcedure);
            tipoCuenta.Id = id;

        }

        public async Task<bool> Existe(string nombre, int usuarioid)
        {
            using SqlConnection connection = new(connectionStrings);
            int existe = await connection.QueryFirstOrDefaultAsync<int>(
                                                   @"SELECT 1
                                                   FROM TiposCuentas
                                                   WHERE Nombre = @Nombre AND UsuarioId = @UsuarioId;", new {nombre, usuarioid});
            return existe == 1;
        }

        public async Task<IEnumerable<TipoCuenta>> Get(int usuarioid)
        {
            using SqlConnection connection = new(connectionStrings);
            
            return await connection.QueryAsync<TipoCuenta>(@"SELECT Id, Nombre, Orden 
                                                             FROM TiposCuentas
                                                             WHERE UsuarioId = @UsuarioId
                                                             ORDER BY Orden", new {usuarioid});
        }

        public async Task Update(TipoCuenta tipoCuenta)
        {
            using SqlConnection connection = new(connectionStrings);

            await connection.ExecuteAsync(@"UPDATE TiposCuentas
                                            SET Nombre = @Nombre
                                            WHERE Id = @Id", tipoCuenta);
        }

        public async Task<TipoCuenta> GetById(int id, int usuarioId)
        {
            using SqlConnection connection = new(connectionStrings);

            TipoCuenta? tipoCuenta = await connection.QueryFirstOrDefaultAsync<TipoCuenta>(@"SELECT Id, Nombre, Orden
                                                          FROM TiposCuentas
                                                          WHERE Id = @Id AND UsuarioId = @UsuarioId", new { id, usuarioId });
            return tipoCuenta;

        }

        public async Task Borrar (int id)
        {
            using SqlConnection connection = new( connectionStrings);
            _ = await connection.ExecuteAsync(@"DELETE TiposCuentas WHERE Id = @Id", new { id });
        }

        public async Task Ordenar(IEnumerable<TipoCuenta> tipoCuentasOrdenados)
        {
            string query = "UPDATE TiposCuentas SET Orden = @Orden Where Id = @Id;";
            using SqlConnection connection = new(connectionStrings);
            _ = await connection.ExecuteAsync(query, tipoCuentasOrdenados);
        }
    }
}
