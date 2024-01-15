using System.Security.Claims;

namespace ControlPresupuesto.Services
{
    public interface IUsersService
    {
        int GetUserid();
    }
    public class UsersService : IUsersService
    {
        private readonly HttpContext httpContext;

        public UsersService(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext;
        }
        public int GetUserid(){

            if (httpContext.User.Identity.IsAuthenticated)
            {
                var idClaim = httpContext.User
                        .Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                var id = int.Parse(idClaim.Value);
                return id;
            }
            else
            {
                throw new ApplicationException("El usuario no está autenticado");
            }
        }
    }
}
