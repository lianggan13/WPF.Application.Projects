using Microsoft.AspNetCore.Mvc.Filters;

namespace YunDa.ASIS.Server.Filters
{
    public class CustomGlobalActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("CustomGlobalActionFilterAttribute.OnActionExecuting");
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("CustomGlobalActionFilterAttribute.OnActionExecuted");
        }
    }
}
