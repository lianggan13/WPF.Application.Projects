using Microsoft.AspNetCore.Mvc.Filters;

namespace YunDa.ASIS.Server.Filters
{
    public class CustomControllerActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("CustomControllerActionFilterAttribute.OnActionExecuting");
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("CustomControllerActionFilterAttribute.OnActionExecuted");
        }
    }
}
