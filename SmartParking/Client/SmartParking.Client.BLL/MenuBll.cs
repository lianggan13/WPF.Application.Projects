using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SmartParking.Client.DAL;
using SmartParking.Client.Model;

namespace SmartParking.Client.BLL
{
    public class MenuBll : IMenuBll
    {
        private readonly IMenuDal menuDal;

        public MenuBll(IMenuDal menuDal)
        {
            this.menuDal = menuDal;
        }

        public async Task<List<sys_menu>> GetMenusByUserId(int userId)
        {
            var menuStr = await menuDal.GetMenusByUserId(userId);
            var menus = JsonConvert.DeserializeObject<List<sys_menu>>(menuStr);
            return menus;
        }
    }
}
