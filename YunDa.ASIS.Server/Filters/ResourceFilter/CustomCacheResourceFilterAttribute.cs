using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace YunDa.ASIS.Server.Filters
{
    public class CustomCacheResourceFilterAttribute : Attribute, IResourceFilter
    {

        //1.定义一个缓存的区域
        //2.请求来了，根据缓存的标识---判断缓存如果有缓存，就返回缓存的值
        //3.如果没有缓存---做计算
        //4.计算结果保存到缓存中去


        private static Dictionary<string, object> CacheDictionary = new Dictionary<string, object>();

        /// <summary>
        /// 在XX资源之前
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuting(ResourceExecutingContext context)
        {

            //throw new Exception("ResourceFilter发生异常了。。");

            //如果要执行匿名：就不让在这里定义的代码执行
            {
                //context.ActionDescriptor.EndpointMetadata
                //包含了标记在当前要访问的Action上/当前Action所在的控制器上标记的所有的特性
                //var endPoint = context.HttpContext.Features.Get<IEndpointFeature>()?.Endpoint;
                //var allow = endPoint.Metadata.OfType<AllowAnonymousAttribute>();

                if (context.ActionDescriptor.EndpointMetadata.Any(c => c.GetType().Equals(typeof(CustomAllowAnonymousAttribute))))
                {
                    return;
                }
            }


            string key = context.HttpContext.Request.Path; //请求的路径
            if (CacheDictionary.ContainsKey(key))
            {
                //只要是给Result赋值了，就会中断往后执行，直接返回给调用方
                context.Result = (IActionResult)CacheDictionary[key];
            }

            Console.WriteLine("CustomResourceFilterAttribute.OnResourceExecuting");
        }

        /// <summary>
        /// 在在XX资源之后
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.Any(c => c.GetType().Equals(typeof(CustomAllowAnonymousAttribute))))
            {
                return;
            }
            string key = context.HttpContext.Request.Path;
            if (context.Result != null)
            {
                CacheDictionary[key] = context.Result;
            }

            Console.WriteLine("CustomResourceFilterAttribute.OnResourceExecuted");
        }
    }
}
