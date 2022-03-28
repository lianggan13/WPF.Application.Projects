using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Security.Claims;
using YunDa.ASIS.Server.Filters.AuthorizeAttr;
using YunDa.ASIS.Server.Models;
using YunDa.ASIS.Server.Services;

namespace YunDa.ASIS.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly MongoDbService dbService;

        public UserController(MongoDbService dbService)
        {
            this.dbService = dbService;
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin,User")]
        [HttpGet]
        public async Task<IActionResult> Query()
        {
            IEnumerable<User> users = dbService.UserColl.Find(_ => true).ToEnumerable();
            return await Task.FromResult(Json(users));
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Policy = AuthorizePolicy.UserPolicy)]
        [HttpGet]
        public async Task<IActionResult> Query2()
        {
            IEnumerable<User> users = dbService.UserColl.Find(_ => true).ToEnumerable();
            return await Task.FromResult(Json(users));
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // Need jwt token: Postman -> Authorization -> Bearer Token
        [HttpGet]
        public async Task<IActionResult> Query3()
        {
            IEnumerable<User> users = dbService.UserColl.Find(_ => true).ToEnumerable();
            return await Task.FromResult(Json(users));
        }


        [HttpGet]
        public async Task<IActionResult> Login()
        {
            //var user = HttpContext.User;
            return await Login(name: "lianggan13", password: "1918");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string name, string password)
        {
            IActionResult? result = null;
            if (name == "lianggan13" && password == "1918")
            {
                var claims = new List<Claim>()//鉴别你是谁，相关信息
                {
                    new Claim("Userid","1"),
                    new Claim(ClaimTypes.Role,"Admin"),
                    new Claim(ClaimTypes.Role,"User"),
                    new Claim(ClaimTypes.Name,$"{name}--来自于Cookies"),
                    new Claim(ClaimTypes.Email,$"18672713698@163.com"),
                    new Claim("password",password),//可以写入任意数据
                    new Claim("Account","Administrator"),
                    new Claim("role","admin"),
                    new Claim("QQ","1030499676"),
                    //new Claim("AuthKey","8888888888888888")
                };
                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Customer"));
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(3),//过期时间：30分钟
                }).Wait();
                result = RedirectToAction(nameof(Query));
            }
            else
            {
                result = RedirectToAction(nameof(Login));
            }

            return await Task.FromResult(result);
        }


        [HttpPost]
        public async Task<IActionResult> UpsertUser([FromBody] User user)
        {
            #region ReplaceOne
            //ReplaceOptions opt = new ReplaceOptions()
            //{
            //    IsUpsert = true // 没有，就插入
            //};
            //ReplaceOneResult reuslt = dbService.UserColl.ReplaceOne(i => i.ID == user.ID, user, opt); 
            #endregion

            var filter = Builders<User>.Filter.Eq(u => u.ID, user.ID);

            var udef = Builders<User>.Update.Set(u => u.No, user.No)
                                            .Set(u => u.Name, user.Name)
                                            .Set(u => u.CardNo, user.CardNo)
                                            .Set(u => u.Photo, user.Photo)
                                            .Set(u => u.RoleId, user.RoleId)
                                            .Set(u => u.UserGroupId, user.UserGroupId)
                                            .Set(u => u.AllowUpdate, true);

            UpdateOptions uopt = new UpdateOptions()
            {
                IsUpsert = true // 没有，就插入
            };

            UpdateResult result = dbService.UserColl.UpdateOne(filter, udef, uopt);
            JsonResult jr = Json(result);
            return await Task.FromResult(jr);
        }

    }
}
