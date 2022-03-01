using Microsoft.AspNetCore.Mvc;
using SmartParking.Server.Service;

namespace SmartParking.Server.Start
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService menuService;

        public MenuController(IMenuService menuService)
        {
            this.menuService = menuService;
        }


        [HttpPost]
        [Route("Load")]
        public IActionResult GetMenusByUserId([FromForm] int userId)
        {
            var menus = menuService.GetMenusByUserId(userId);
            if (menus != null)
            {
                return Ok(menus);
            }
            else
            {
                return NoContent();
            }
        }
    }
}
