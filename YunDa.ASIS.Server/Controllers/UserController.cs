using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using YunDa.ASIS.Server.Filters;
using YunDa.ASIS.Server.Models;
using YunDa.ASIS.Server.Services;

namespace YunDa.ASIS.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [TypeFilter(typeof(CustomAllActionResultFilterAttribute))]
    public class UserController : Controller
    {
        private readonly MongoDbService dbService;

        public UserController(MongoDbService dbContext)
        {
            this.dbService = dbContext;

        }
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<User> users = dbService.UserColl.Find((u) => true).ToEnumerable();
            return Json(users);
        }
    }
}
