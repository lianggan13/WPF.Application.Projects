using Microsoft.AspNetCore.Mvc.Filters;

namespace YunDa.ASIS.Server.Filters
{
    public class CustomLogActionFilterAttribute : Attribute, IActionFilter
    {
        private readonly ILogger<CustomLogActionFilterAttribute> _ILogger;
        public CustomLogActionFilterAttribute(ILogger<CustomLogActionFilterAttribute> iLogger)
        { 
            this._ILogger= iLogger;
        }


        /// <summary>
        /// 在XXAction执行之前
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnActionExecuting(ActionExecutingContext context)
        {

            //throw new Exception("ActionFilter发生异常了。。。");

            var para= context.HttpContext.Request.QueryString.Value;
            var controllerName = context.HttpContext.GetRouteValue("controller");
            var actionName = context.HttpContext.GetRouteValue("action");
            _ILogger.LogInformation($"执行{controllerName}控制器--{actionName}方法；参数为：{para}");
        }

        /// <summary>
        /// 在XXAction执行之后
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var result =Newtonsoft.Json.JsonConvert.SerializeObject(context.Result);
            var controllerName = context.HttpContext.GetRouteValue("controller");
            var actionName = context.HttpContext.GetRouteValue("action");
            _ILogger.LogInformation($"执行{controllerName}控制器--{actionName}方法:执行结果为：{result}");
        }
    }
}
