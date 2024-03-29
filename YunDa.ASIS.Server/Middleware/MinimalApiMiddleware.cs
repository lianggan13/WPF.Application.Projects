﻿using System.Net;

namespace YunDa.ASIS.Server.Middleware
{
    public class MinimalApiMiddlewareData
    {
        // add configuration properties if needed
    }

    public class MinimalApiMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MinimalApiMiddleware> _ILogger;

        public MinimalApiMiddleware(RequestDelegate next, ILogger<MinimalApiMiddleware> iLogger)
        {
            _next = next;
            this._ILogger = iLogger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.GetEndpoint()?.Metadata.GetMetadata<MinimalApiMiddlewareData>() is { } mutateResponseMetadata)
            {
                using (var scope = httpContext.RequestServices.CreateScope())
                {
                    //var filter = scope.ServiceProvider.GetService<CustomAllActionResultFilterAttribute>();
                    IPAddress? remoteIpAddress = httpContext.Request.HttpContext.Connection.RemoteIpAddress;

                    _ILogger.LogInformation($">> [{DateTime.Now}] {remoteIpAddress} {httpContext.Request.Method} {httpContext.Request.Path}");
                }
            }

            await _next(httpContext);
        }
    }
}
