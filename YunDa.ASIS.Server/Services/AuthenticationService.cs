using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace YunDa.ASIS.Server.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string? scheme)
        {
            throw new NotImplementedException();
        }

        public Task ChallengeAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }

        public Task ForbidAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }

        public Task SignInAsync(HttpContext context, string? scheme, ClaimsPrincipal principal, AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }

        public Task SignOutAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }
    }

    //public class AuthenticationService2 : IAuthenticationService
    //{
    //    public IAuthenticationSchemeProvider Schemes { get; }
    //    public IAuthenticationHandlerProvider Handlers { get; }
    //    public IClaimsTransformation Transform { get; }

    //    public virtual async Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme)
    //    {
    //        if (scheme == null)
    //        {
    //            var scheme = (await this.Schemes.GetDefaultAuthenticateSchemeAsync())?.Name;
    //            if (scheme == null)
    //                throw new InvalidOperationException($"No authenticationScheme was specified, and there was no DefaultAuthenticateScheme found.");
    //        }

    //        var handler = await Handlers.GetHandlerAsync(context, scheme);
    //        if (handler == null)
    //            throw await this.CreateMissingHandlerException(scheme);
    //        AuthenticateResult result = await handler.AuthenticateAsync();
    //        if (result != null && result.Succeeded)
    //            return AuthenticateResult.Success(new AuthenticationTicket(await Transform.TransformAsync(result.Principal), result.Properties, result.Ticket.AuthenticationScheme));

    //        return result;
    //    }

    //}
}
