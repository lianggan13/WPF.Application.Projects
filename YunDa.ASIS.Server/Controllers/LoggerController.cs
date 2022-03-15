using Microsoft.AspNetCore.Mvc;

namespace YunDa.ASIS.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoggerController : ControllerBase
    {
        private readonly ILogger<LoggerController> _Logger;
        private readonly ILoggerFactory _LoggerFactory;
        public LoggerController(ILogger<LoggerController> logger, ILoggerFactory loggerFactory)
        {
            this._Logger = logger;
            this._Logger.LogInformation($"{this.GetType().Name} 被构造了。。。_Logger");

            this._LoggerFactory = loggerFactory;
            ILogger<LoggerController> _Logger2 = this._LoggerFactory.CreateLogger<LoggerController>();
            _Logger2.LogInformation($"{this.GetType().Name} 被构造了。。。_Logger2");
        }

        [HttpGet]
        public IActionResult LoginIndex()
        {
            ILogger<LoggerController> _Logger3 = this._LoggerFactory.CreateLogger<LoggerController>();
            _Logger3.LogInformation($"Index 被执行了。。。。。_Logger3");
            this._Logger.LogInformation($"Index 被执行了。。。");

            return Ok();
        }

        [HttpGet]
        [Route("/[controller]/level")]
        public IActionResult Level()
        {
            _Logger.LogDebug("this is Debug");
            _Logger.LogInformation("this is Info");
            _Logger.LogWarning("this is Warn");
            _Logger.LogError("this is Error");
            _Logger.LogTrace("this is Trace");
            _Logger.LogCritical("this is Critical");

            return new JsonResult(new { Success = true });
        }

        //[HttpGet]
        //public object GetData()
        //{
        //    return new
        //    {
        //        Id = 123,
        //        Name = "Richard"
        //    };
        //}
    }
}
