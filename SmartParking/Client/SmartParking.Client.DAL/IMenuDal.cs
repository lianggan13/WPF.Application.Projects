using System.Threading.Tasks;

namespace SmartParking.Client.DAL
{
    public interface IMenuDal
    {
        Task<string> GetMenusByUserId(int userId);
    }
}