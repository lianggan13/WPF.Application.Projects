using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SmartParking.Server.DAL.EFCore;
using SmartParking.Server.Model;

namespace SmartParking.Server.Service
{
    public class UserService : BaseService, IUserService
    {
        public UserService(MySqlDbContext context) : base(context)
        {

        }

        public sys_user Login(string userName, string password)
        {
            sys_user user = context.sys_user.FirstOrDefault(u => (u.username == userName || u.nickname == userName) && u.password == password);
            return user;
        }

        public List<sys_user> GetUsers()
        {
            var users = context.sys_user.Where(s => s.status == 0)?.ToList();
            return users;
        }

        public sys_user SetState(string userId, string state)
        {
            //var user = this.Find<sys_user>(int.Parse(userId));
            var user = this.Find<sys_user>(long.Parse(userId));
            user.status = sbyte.Parse(state);
            this.Update<sys_user>(user);
            return user;
        }

        public sys_user SaveUser(string userStr)
        {
            var user = JsonConvert.DeserializeObject<sys_user>(userStr);
            this.Update<sys_user>(user);
            return user;
        }

        public List<sys_role> GetRolesByUserId(string userId)
        {
            //context.sys_user_role.Where(ur => ur.user_id == long.Parse(userId));
            var ss = from ur in context.sys_user_role
                     where ur.user_id == long.Parse(userId)
                     join r in context.sys_role
                     on ur.role_id equals r.role_id
                     select r;
            return ss?.ToList();
        }
    }
}