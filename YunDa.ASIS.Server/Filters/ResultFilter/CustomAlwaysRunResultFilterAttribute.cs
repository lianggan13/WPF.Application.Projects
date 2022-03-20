using Microsoft.AspNetCore.Mvc.Filters;

namespace YunDa.ASIS.Server.Filters
{
    public class CustomAlwaysRunResultFilterAttribute : Attribute, IAlwaysRunResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            //对于本次请求，取到了缓存？
            //还想要获取一些非缓存的数据的时候： 
            Console.WriteLine("CustomAlwaysRunResultFilterAttribute.OnResultExecuting");
        }
        public void OnResultExecuted(ResultExecutedContext context)
        {
            Console.WriteLine("CustomAlwaysRunResultFilterAttribute.OnResultExecuted");
        }
    }
}
