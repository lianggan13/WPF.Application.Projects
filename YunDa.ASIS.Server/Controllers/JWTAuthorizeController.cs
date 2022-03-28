using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YunDa.ASIS.Server.Services.JWT;

namespace YunDa.ASIS.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtAuthorizeController : ControllerBase
    {
        private IJWTAuthorizeService _iJWTService = null;
        public JwtAuthorizeController(IJWTAuthorizeService customJWTService)
        {
            _iJWTService = customJWTService;
        }

        [Route("Login")]
        [HttpPost]
        public string Login(string name, string password)
        {
            //在这里需要去数据库中做数据验证
            if ("lianggan13".Equals(name) && "1918".Equals(password))
            {
                //就应该生成Token 
                string token = this._iJWTService.GetToken(name, password);
                return JsonConvert.SerializeObject(new
                {
                    result = true,
                    token
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new
                {
                    result = false,
                    token = ""
                });
            }
        }
    }
}
