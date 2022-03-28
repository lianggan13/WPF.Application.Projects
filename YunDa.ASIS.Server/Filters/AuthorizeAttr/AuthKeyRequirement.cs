using Microsoft.AspNetCore.Authorization;

namespace YunDa.ASIS.Server.Filters.AuthorizeAttr
{
    public class AuthKeyRequirement : IAuthorizationRequirement
    {
        public const string AuthKey = nameof(AuthKey);
    }

    public class AuthKeyHander : AuthorizationHandler<AuthKeyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthKeyRequirement requirement)
        {
            var httpContext = context.Resource as HttpContext;
            var ss = httpContext.Request.Headers[AuthKeyRequirement.AuthKey];
            var key = ss.ElementAtOrDefault(0);

            if (key == "8888888888888888")
            {
                context.Succeed(requirement); // pass
            }

            return Task.CompletedTask;
        }
    }
}
