using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace YunDa.ASIS.Server.Services.JWT
{
    public class JWTAuthorizHSService : IJWTAuthorizeService
    {
        #region Option注入
        private readonly JWTTokenOptions _JWTTokenOptions;
        public JWTAuthorizHSService(IOptionsMonitor<JWTTokenOptions> jwtTokenOptions)
        //public CustomHSJWTService(IOptions<JWTTokenOptions> jwtTokenOptions)
        {
            this._JWTTokenOptions = jwtTokenOptions.CurrentValue;
        }
        #endregion
        /// <summary>
        /// 用户登录成功以后，用来生成Token的方法
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public string GetToken(string UserName, string password)
        {
            #region 有效载荷，大家可以自己写，爱写多少写多少；尽量避免敏感信息
            var claims = new[]
            {
               new Claim(ClaimTypes.Name, UserName),
                new Claim(ClaimTypes.Role, "teache0"),
               new Claim("NickName",UserName),
               new Claim("Role","Administrator"),//传递其他信息   
               new Claim("ABCC","ABCC"),
               new Claim("Student","甜酱油")
            };

            //需要加密：需要加密key:
            //Nuget引入：Microsoft.IdentityModel.Tokens
            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_JWTTokenOptions.SecurityKey));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Nuget引入：System.IdentityModel.Tokens.Jwt
            JwtSecurityToken token = new JwtSecurityToken(
             issuer: _JWTTokenOptions.Issuer,
             audience: _JWTTokenOptions.Audience,
             claims: claims,
             expires: DateTime.Now.AddMinutes(3),//5分钟有效期
             signingCredentials: creds);

            string returnToken = new JwtSecurityTokenHandler().WriteToken(token);
            return returnToken;
            #endregion

        }
    }
}
