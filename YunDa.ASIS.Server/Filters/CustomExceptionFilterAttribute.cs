using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace YunDa.ASIS.Server.Filters
{
    public class CustomExceptionFilterAttribute : Attribute, IExceptionFilter //, IAsyncExceptionFilter
    {

        private readonly IModelMetadataProvider _IModelMetadataProvider;
        public CustomExceptionFilterAttribute(IModelMetadataProvider modelMetadataProvider)
        {
            this._IModelMetadataProvider = modelMetadataProvider;
        }

        /// <summary>
        /// 当有异常发生的时候，就会触发到这里
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled == false)
            {
                //在这里就开始处理异常--还是要响应结果给客户端
                //1.页面展示
                //2.包装成一个JSON格式
                if (IsAjaxRequest(context.HttpContext.Request)) //判断是否是Ajax请求--JSON
                {
                    //JSON返回
                    context.Result = new JsonResult(new
                    {
                        Succeess = false,
                        Message = context.Exception.Message
                    });
                }
                else
                {
                    //返回页面
                    ViewResult result = new ViewResult { ViewName = "~/Views/Shared/Error.cshtml" };
                    result.ViewData = new ViewDataDictionary(_IModelMetadataProvider, context.ModelState);
                    result.ViewData.Add("Exception", context.Exception);
                    context.Result = result; //断路器---只要对Result赋值--就不继续往后了； 
                }
                context.ExceptionHandled = true;//表示当前异常被处理过
            }
        }


        ///// <summary>
        ///// 当有异常发生的时候，就会触发到这里
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        ///// <exception cref="NotImplementedException"></exception>
        //public Task OnExceptionAsync(ExceptionContext context)
        //{
        //    throw new NotImplementedException();
        //}


        private bool IsAjaxRequest(HttpRequest request)
        {
            //HttpWebRequest httpWebRequest = null;
            //httpWebRequest.Headers.Add("X-Requested-With", "XMLHttpRequest"); 
            string header = request.Headers["X-Requested-With"];
            return "XMLHttpRequest".Equals(header);
        }
    }
}
