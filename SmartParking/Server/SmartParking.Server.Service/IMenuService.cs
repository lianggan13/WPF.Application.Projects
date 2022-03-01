using System.Collections.Generic;
using SmartParking.Server.Model;

namespace SmartParking.Server.Service
{
    public interface IMenuService : IBaseService
    {
        List<sys_menu> GetMenus();
        List<sys_menu> GetMenusByRoleId(int roleId);
        List<sys_menu> GetMenusByUserId(int userId);
    }
}