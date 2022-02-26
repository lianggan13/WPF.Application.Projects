using Microsoft.AspNetCore.Http.Features;

namespace YunDa.ASIS.Server.Middleware
{
    public class LogsMiddleware
    {
        private readonly RequestDelegate _next;

        public LogsMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            if (endpoint == null)
            {
                await _next(context);
                return;
            }

            using (var scope = context.RequestServices.CreateScope())
            {
                var _logger = scope.ServiceProvider.GetService<ILogger<LogsMiddleware>>();

                var attruibutes = endpoint.Metadata.OfType<NoLogsAttriteFilter>();
                if (attruibutes.Count() == 0)
                {
                    _logger.LogInformation($" url:{context.Request.Path}, 访问时间:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                }

                //记录 排除的特殊Message 信息
                foreach (var attribute in attruibutes)
                {
                    _logger.LogInformation(attribute.Message);
                }
            }
            await _next(context);
        }
    }

    public class NoLogsAttriteFilter : Attribute
    {
        /// <summary>
        /// 这里加这个主要是把获取到的信息在中间件中打印出来，区分中间件的拦截用处
        /// </summary>
        public string Message = "";

        public NoLogsAttriteFilter(string message)
        {
            Message = message;
        }
    }

}
