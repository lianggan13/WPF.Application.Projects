using System.Collections.Generic;
using SmartParking.Server.Model;

namespace SmartParking.Server.Service
{
    public interface IUserService : IBaseService
    {
        sys_user Login(string userName, string password);
        List<sys_user> GetUsers();
        sys_user SetState(string userId, string state);
        sys_user SaveUser(string userStr);
        List<sys_role> GetRolesByUserId(string userId);
    }
}
