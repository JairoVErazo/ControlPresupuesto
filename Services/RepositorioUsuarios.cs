using ControlPresupuesto.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ControlPresupuesto.Services
{
    public interface IRepositorioUsuarios
    {
        Task<Usuario> BuscarUsuarioPorEmail(string emailNormalizado);
        Task<int> CrearUsuario(Usuario usuario);
    }
    public class RepositorioUsuarios : IRepositorioUsuarios
    {
        private readonly string connectionStrings;
        public RepositorioUsuarios( IConfiguration configuration)
        {
            connectionStrings = configuration.GetConnectionString("DefaultConnection");
       
        }

        public async Task<int> CrearUsuario(Usuario usuario)
        {
            using SqlConnection connection = new SqlConnection(connectionStrings);
            var usuarioId = await connection.QuerySingleAsync<int>(@"
                        INSERT INTO Usuario (Email, EmailNormalizado, PasswordHash)
                        VALUES (@Email, @EmailNormalizado, @PasswordHash);
                        SELECT SCOPE_IDENTITY();
                        ", usuario);

            await connection.ExecuteAsync("CrearDatosUsuarioNuevo", new { usuarioId },
                commandType: System.Data.CommandType.StoredProcedure);

            return usuarioId;
        }

        public async Task<Usuario> BuscarUsuarioPorEmail(string emailNormalizado)
        {
            using SqlConnection connection = new SqlConnection(connectionStrings);
            Usuario usuario = await connection.QuerySingleOrDefaultAsync<Usuario>(
                "SELECT * FROM Usuario Where EmailNormalizado = @emailNormalizado",
                new { emailNormalizado });
            return usuario;
        }
    }
}
