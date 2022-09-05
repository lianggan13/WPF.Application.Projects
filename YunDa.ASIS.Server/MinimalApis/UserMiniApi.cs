using MongoDB.Driver;
using YunDa.ASIS.Server.Models;


namespace YunDa.ASIS.Server.MinimalApis
{
    public static class UserMiniApi
    {
        public static void UseUserMiniApi(this WebApplication app)
        {
            app.MapGet("/api/user/query4",
            //[Authorize(AuthorizePolicy.UserPolicy)]
            //[TypeFilter(typeof(CustomAllActionResultFilterAttribute))]
            async (HttpContext httpContext, MongoDbService dbService) =>
                {
                    IEnumerable<User> users = dbService.UserColl.Find(_ => true).ToEnumerable();

                    //return await Task.CompletedTask;
                    return await Task.FromResult(JsonConvert.SerializeObject(users));
                    //return new JsonResult(users);
                })
               .WithTags("User")
               .WithMetadata(new MinimalApiMiddlewareData())
               .RequireAuthorization(new MinimalApiuthorizeData()
               {
                   AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
                   Roles = "teache0"
               });
        }
    }

    public class MinimalApiuthorizeData : IAuthorizeData
    {
        public string? Policy { get; set; }
        public string? Roles { get; set; }
        public string? AuthenticationSchemes { get; set; }
    }
}




