using System.Collections.Generic;
using System.Threading.Tasks;
using SmartParking.Client.Model;

namespace SmartParking.Client.BLL
{
    public interface IMenuBll
    {
        Task<List<sys_menu>> GetMenusByUserId(int userId);
    }
}