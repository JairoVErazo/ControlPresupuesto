namespace ControlPresupuesto.Services
{
    public interface IUsersService
    {
        int GetUserid();
    }
    public class UsersService : IUsersService
    {
        public int GetUserid(){

            return 1;
        }
    }
}
