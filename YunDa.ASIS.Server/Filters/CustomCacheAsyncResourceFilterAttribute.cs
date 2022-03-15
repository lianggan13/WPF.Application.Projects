using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace YunDa.ASIS.Server.Filters
{
    public class CustomCacheAsyncResourceFilterAttribute : Attribute, IAsyncResourceFilter
    {

        private static Dictionary<string, object> CacheDictionary = new Dictionary<string, object>();


        /// <summary>
        /// 当XXX资源去执行的时候
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            Console.WriteLine("CustomCacheAsyncResourceFilterAttribute.OnResourceExecutionAsync Before");

            string key = context.HttpContext.Request.Path;
            if (CacheDictionary.ContainsKey(key))
            {
                context.Result = (IActionResult)CacheDictionary[key];
            }
            else
            {
                ResourceExecutedContext resource = await next.Invoke(); //这句话的执行就是去执行控制器的构造函数+Action方法
                if (resource.Result!=null)
                {
                    CacheDictionary[key] = resource.Result;
                }
               
                Console.WriteLine("CustomCacheAsyncResourceFilterAttribute.OnResourceExecutionAsync After");
            }




        }
    }
}
