using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SmartParking.Client.DAL;
using SmartParking.Client.Model;

namespace SmartParking.Client.BLL
{
    public class UserBll : IUserBll
    {
        private readonly IUserDal userDal;

        public UserBll(IUserDal loginDal)
        {
            this.userDal = loginDal;
        }

        public async Task<string> Login(string userName, string password)
        {
            var user = await userDal.Login(userName, password);
            return user;
        }

        public async Task<List<sys_user>> GetUsers()
        {
            var userStr = await userDal.GetUsers();//.GetAwaiter().GetResult();
            var users = JsonConvert.DeserializeObject<List<sys_user>>(userStr);
            return users;
        }

        public async Task SetState(long userId, int state)
        {
            await userDal.SetState(userId, state);
        }

        public async Task SaveUser(sys_user user)
        {
            await userDal.SaveUser(JsonConvert.SerializeObject(user));
        }

        public async Task<List<sys_role>> GetRolesByUserId(long user_id)
        {
            var rolesStr = await userDal.GetRolesByUserId(user_id);
            var roles = JsonConvert.DeserializeObject<List<sys_role>>(rolesStr);
            return roles;
        }
    }
}
