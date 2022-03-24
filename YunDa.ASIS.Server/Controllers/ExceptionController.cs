using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace YunDa.ASIS.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExceptionController : ControllerBase
    {
        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int s)
        {
            string? RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return Ok(RequestId);
        }




        [HttpGet]
        public IActionResult Throw()
        {
            throw new Exception("Error: " + HttpContext.Request.Path.Value);
        }

        [HttpGet]
        public IActionResult UnAuthorized()
        {
            return Ok("UnAuthorized");
        }


        //[Route("Error/{statusCode}")]
        //public IActionResult HandleErrorCode(int statusCode)
        //{
        //    var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

        //    //switch (statusCode)
        //    //{
        //    //    case 404:
        //    //        ViewBag.ErrorMessage = "Sorry the page you requested could not be found";
        //    //        ViewBag.RouteOfException = statusCodeData.OriginalPath;
        //    //        break;
        //    //    case 500:
        //    //        ViewBag.ErrorMessage = "Sorry something went wrong on the server";
        //    //        ViewBag.RouteOfException = statusCodeData.OriginalPath;
        //    //        break;
        //    //}

        //    return Ok();
        //}
    }
}
