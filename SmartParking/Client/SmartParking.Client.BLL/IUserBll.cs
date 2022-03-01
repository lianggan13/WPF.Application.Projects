using System.Collections.Generic;
using System.Threading.Tasks;
using SmartParking.Client.Model;

namespace SmartParking.Client.BLL
{
    public interface IUserBll
    {
        Task<List<sys_user>> GetUsers();
        Task<string> Login(string userName, string password);
        Task SetState(long userId, int state);
        Task SaveUser(sys_user user);
        Task<List<sys_role>> GetRolesByUserId(long user_id);
    }
}