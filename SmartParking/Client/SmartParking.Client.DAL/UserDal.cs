using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartParking.Client.DAL
{
    public class UserDal : WebDataAccess, IUserDal
    {
        public Task<string> Login(string userName, string password)
        {
            Dictionary<string, HttpContent> contents = new Dictionary<string, HttpContent>();
            contents.Add("userName", new StringContent(userName));
            contents.Add("password", new StringContent(password));

            return this.PostDatas("user/login", contents);
        }

        public Task<string> GetUsers()
        {
            return this.GetDatas("user/all");
        }

        public Task<string> GetRolesByUserId(long userId)
        {
            return this.GetDatas($"user/roles/{userId}");
        }

        public Task SetState(long userId, int state)
        {
            Dictionary<string, HttpContent> contents = new Dictionary<string, HttpContent>();
            contents.Add("userId", new StringContent($"{userId}"));
            contents.Add("state", new StringContent($"{state}"));

            return this.PostDatas("user/setstate", contents);
        }

        public Task SaveUser(string userStr)
        {
            StringContent content = new StringContent(userStr);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            return this.PostDatas("user/save", content);
        }
    }
}
