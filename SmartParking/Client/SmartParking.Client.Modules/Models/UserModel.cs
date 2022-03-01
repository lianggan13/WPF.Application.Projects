using System.Collections.ObjectModel;
using Prism.Mvvm;
using SmartParking.Client.Model;

namespace SmartParking.Client.Modules.Models
{
    public class UserModel : BindableBase
    {
        public UserModel(sys_user user)
        {
            SysUser = user;
        }
        public sys_user SysUser { get; }
        public string UserIcon { get; set; }
        public ObservableCollection<sys_role> Roles { get; set; } = new ObservableCollection<sys_role>();
    }
}
