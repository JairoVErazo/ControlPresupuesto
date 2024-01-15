using ControlPresupuesto.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ControlPresupuesto.Services
{
    public interface IRepositorioCuentas
    {
        Task Actualizar(CuentaCreateModelView cuenta);
        Task Borrar(int id);
        Task<IEnumerable<Cuenta>> Buscar(int usuarioId);
        Task Crear(Cuenta cuenta);
        Task<Cuenta> GetById(int id, int usuarioId);
    }

    public class RepositorioCuentas : IRepositorioCuentas
    {
        private readonly string connectionStrings;
        public RepositorioCuentas(IConfiguration configuration)
        {
            connectionStrings = configuration.GetConnectionString("DefaultConnection");

        }

        public async Task Crear(Cuenta cuenta)
        {
            using var connection = new SqlConnection(connectionStrings);
            var id = await connection.QuerySingleAsync<int>(
                @"INSERT INTO Cuentas (Nombre, TipoCuentaId, Descripcion, Balance)
                    VALUES (@Nombre, @TipoCuentaId, @Descripcion, @Balance);

                    SELECT SCOPE_IDENTITY();", cuenta);

            cuenta.Id = id;
        }

        public async Task<IEnumerable<Cuenta>>Buscar (int usuarioId)
        {
            using SqlConnection connection = new(connectionStrings);
            return await connection.QueryAsync<Cuenta>(@"SELECT Cuentas.Id, Cuentas.Nombre, Balance, tc.Nombre AS TipoCuenta
                                                        FROM Cuentas
                                                        INNER JOIN TiposCuentas tc
                                                        ON tc.Id = Cuentas.TipoCuentaId
                                                        WHERE tc.UsuarioId = @UsuarioId
                                                        ORDER BY tc.Orden", new { usuarioId });
        }

        public async Task<Cuenta> GetById(int id, int usuarioId)
        {
            using SqlConnection connection = new(connectionStrings);
            return await connection.QueryFirstOrDefaultAsync<Cuenta>(
              @"SELECT Cuentas.Id, Cuentas.Nombre, Balance, Descripcion, TipoCuentaId
                FROM Cuentas
                INNER JOIN TiposCuentas tc
                ON tc.Id = Cuentas.TipoCuentaId
                WHERE tc.UsuarioId = @UsuarioId AND Cuentas.Id = @Id", new { id, usuarioId });
        }

        public async Task Actualizar(CuentaCreateModelView cuenta)
        {
            using SqlConnection connection = new(connectionStrings);
            await connection.ExecuteAsync(@"UPDATE Cuentas
                                    SET Nombre = @Nombre, Balance = @Balance, Descripcion = @Descripcion,
                                    TipoCuentaId = @TipoCuentaId
                                    WHERE Id = @Id;", cuenta);
        }

        public async Task Borrar(int id)
        {
            using SqlConnection connection = new(connectionStrings);
            await connection.ExecuteAsync("DELETE Cuentas WHERE Id = @Id", new { id });
        }
    }
}
