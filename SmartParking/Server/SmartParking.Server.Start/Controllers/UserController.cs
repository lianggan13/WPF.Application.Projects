using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParking.Server.Service;

namespace SmartParking.Server.Start.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService userService;

        public UserController(IUserService loginService)
        {
            this.userService = loginService;
        }


        [HttpGet]
        [Route("all")]
        public IActionResult GetUsers()
        {
            var users = userService.GetUsers();
            if (users != null)
            {
                return Ok(users);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet]
        [Route("roles/{userId}")]
        public IActionResult GetRolesByUserId(long userId)
        {
            var roles = userService.GetRolesByUserId(userId.ToString());
            if (roles != null)
            {
                return Ok(roles);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromForm] string username, [FromForm] string password)
        {
            //password = GetMD5Str(password);
            var user = userService.Login(username, password);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPost]
        [Route("setstate")]
        public IActionResult SetState([FromForm] string userId, [FromForm] string state)
        {
            //var user = userService.SetState(long.Parse(userId), int.Parse(state));
            var user = userService.SetState(userId, state);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPost]
        [Route("resetpwd")]
        public IActionResult ResetPassword([FromForm] IFormCollection form)
        {
            //userService.ResetPassword(int.Parse(form["userId"]));
            return Ok();
        }


        [HttpPost]
        [Route("save")]
        public IActionResult SaveUser([FromBody] JsonElement userStr)
        {
            var user = userService.SaveUser(userStr.ToString());
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NoContent();
            }
        }

        public string GetMD5Str(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr)) return "";

            byte[] result = Encoding.Default.GetBytes(inputStr);    //tbPass为输入密码的文本框
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", "");  //tbMd5pass为输出加密文本的文本框
        }
    }
}
