using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace YunDa.ASIS.Server.Providers
{
    public class CustomAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions options;

        public CustomAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            this.options = options.Value;
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return Task.FromResult(options.DefaultPolicy);
        }

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        {

            return Task.FromResult(options.FallbackPolicy);
        }

        public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            // 判断 AuthorizationOptions 是否包含当前的 policy
            AuthorizationPolicy? policy = options.GetPolicy(policyName);
            if (policy != null)
            {
                //return policy;
                return await Task.FromResult(policy);
            }
            string[] cliams = policyName.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            options.AddPolicy(policyName, builder =>
            {
                builder.RequireClaim(cliams[0], cliams[1]);
            });

            return await Task.FromResult(options.GetPolicy(policyName));
        }
    }
}
