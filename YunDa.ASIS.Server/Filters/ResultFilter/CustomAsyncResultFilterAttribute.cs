using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using YunDa.ASIS.Server.Models;

namespace YunDa.ASIS.Server.Filters
{
    public class CustomAsyncResultFilterAttribute : Attribute, IAsyncResultFilter
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            {
                if (context.Result is JsonResult)
                {
                    JsonResult result = (JsonResult)context.Result;
                    context.Result = new JsonResult(new AjaxResult()
                    {
                        Success = true,
                        Message = "OK",
                        Data = result.Value
                    });
                }
            }
            await next.Invoke();
        }
    }
}
