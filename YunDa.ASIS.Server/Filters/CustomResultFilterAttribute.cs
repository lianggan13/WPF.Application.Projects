using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using YunDa.ASIS.Server.Models;

namespace YunDa.ASIS.Server.Filters
{
    public class CustomResultFilterAttribute : Attribute, IResultFilter
    {

        /// <summary>
        /// 在xx结果之前
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnResultExecuting(ResultExecutingContext context)
        {
            //throw new Exception("Result 中发生异常。。。");


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

            Console.WriteLine("CustomResultFilterAttribute.OnResultExecuting");
        }

        /// <summary>
        /// /在xx结果之后
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnResultExecuted(ResultExecutedContext context)
        {

        }
    }
}
