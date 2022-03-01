using System.Collections.Generic;

namespace SmartParking.Client.Model
{
    public class GlobalInfo
    {
        public static sys_user CurrentUser { get; set; }
        public static List<sys_menu> CurrentMenus { get; set; }
    }
}
