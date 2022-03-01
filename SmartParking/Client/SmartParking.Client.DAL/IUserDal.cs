using System.Threading.Tasks;

namespace SmartParking.Client.DAL
{
    public interface IUserDal
    {
        Task<string> GetUsers();
        Task<string> Login(string userName, string password);
        Task SetState(long userId, int state);
        Task SaveUser(string userStr);
        Task<string> GetRolesByUserId(long user_id);
    }
}