using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace YunDa.ASIS.Server.Filters
{
    public class CustomCacheActionFilterAttribute : Attribute, IActionFilter
    {
        private static Dictionary<string, object> CacheDictionary = new Dictionary<string, object>();
         
        public void OnActionExecuting(ActionExecutingContext context)
        {

            if (context.ActionDescriptor.EndpointMetadata.Any(c => c.GetType().Equals(typeof(CustomAllowAnonymousAttribute))))
            {
                return;
            }

            string key = context.HttpContext.Request.Path; //请求的路径
            if (CacheDictionary.ContainsKey(key))
            {
                //只要是给Result赋值了，就会中断往后执行，直接返回给调用方
                context.Result = (IActionResult)CacheDictionary[key];
            } 
        }


        public void OnActionExecuted(ActionExecutedContext context)
        {

            if (context.ActionDescriptor.EndpointMetadata.Any(c => c.GetType().Equals(typeof(CustomAllowAnonymousAttribute))))
            {
                return;
            }

            string key = context.HttpContext.Request.Path;
            if (context.Result!=null)
            {
                CacheDictionary[key] = context.Result;
            }
           
        }
    }
}
