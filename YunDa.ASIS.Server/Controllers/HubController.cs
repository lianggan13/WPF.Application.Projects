using Microsoft.AspNetCore.SignalR;
using SignalRNotify;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YunDa.ASIS.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HubController : Controller
    {
        // GET: api/<HubController>
        private readonly IHubContext<NotificationHub> _hubContext;

        public HubController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await _hubContext.Clients.All.SendAsync("Notify", $"Home page loaded at: {DateTime.Now}");
            return Ok();
        }

        [HttpGet]
        [Route("create")]
        public ActionResult Create()
        {
            return LocalRedirect("/Index");
            return View("/Pages/Index.cshtml");
        }
    }
}
