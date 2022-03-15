using Microsoft.AspNetCore.Mvc.Filters;

namespace YunDa.ASIS.Server.Filters
{
    public class CustomLogAsyncActionFilterAttribute : Attribute, IAsyncActionFilter
    {

        private readonly ILogger<CustomLogAsyncActionFilterAttribute> _ILogger;
        public CustomLogAsyncActionFilterAttribute(ILogger<CustomLogAsyncActionFilterAttribute> iLogger)
        {
            this._ILogger = iLogger;
        }
         

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        { 
            var controllerName = context.HttpContext.GetRouteValue("controller");
            var actionName = context.HttpContext.GetRouteValue("action");

            var para = context.HttpContext.Request.QueryString.Value; 
            _ILogger.LogInformation($"执行{controllerName}控制器--{actionName}方法；参数为：{para}");

            ActionExecutedContext executedContext = await next.Invoke(); //这句话执行就是去执行Action  

            var result = Newtonsoft.Json.JsonConvert.SerializeObject(executedContext.Result); 
            _ILogger.LogInformation($"执行{controllerName}控制器--{actionName}方法:执行结果为：{result}");
        }
    }
}
