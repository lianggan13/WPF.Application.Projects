using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartParking.Client.DAL
{
    public class MenuDal : WebDataAccess, IMenuDal
    {
        public Task<string> GetMenusByUserId(int userId)
        {
            Dictionary<string, HttpContent> contents = new Dictionary<string, HttpContent>();
            contents.Add("userId", new StringContent(userId.ToString()));

            return this.PostDatas("menu/load", contents);
        }
    }
}
