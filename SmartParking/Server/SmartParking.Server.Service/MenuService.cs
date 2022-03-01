using System;
using System.Collections.Generic;
using System.Linq;
using SmartParking.Server.DAL.EFCore;
using SmartParking.Server.Model;

namespace SmartParking.Server.Service
{
    public class MenuService : BaseService, IMenuService
    {
        public MenuService(MySqlDbContext context) : base(context)
        {

        }

        public List<sys_menu> GetMenus()
        {
            List<sys_menu> menus = context.sys_menu.ToList();
            return menus;
        }

        public List<sys_menu> GetMenusByRoleId(int roleId)
        {
            //  role --> role_menu --> menu
            var roleMenus = context.sys_role_menu.Where(rm => rm.role_id == roleId);
            var menus = from menu in context.sys_menu
                        join role_menu in roleMenus
                        on menu.menu_id equals role_menu.menu_id
                        select menu;

            return menus.ToList();
        }

        public List<sys_menu> GetMenusByUserId(int userId)
        {
            // user --> user_role --> role --> role_menu --> menu
            var roleIds = context.sys_user_role.Where(ur => ur.user_id == userId).Select(ur => ur.role_id);
            var menus = from menu in context.sys_menu
                        join role_menu in context.sys_role_menu
                        on menu.menu_id equals role_menu.menu_id
                        where roleIds.Contains(role_menu.role_id)
                        select menu;

            return menus.ToList();
        }
    }
}
