using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YunDa.ASIS.Server.Middleware;
using YunDa.ASIS.Server.Models;
using YunDa.ASIS.Server.Services;

namespace YunDa.ASIS.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BooksService _booksService;

        public BooksController(BooksService booksService)
        {
            _booksService = booksService;

        }

        [Authorize(Policy = "testpolicy")] // 可直接放置在控制上
        //[Authorize(Roles = "admin,user")]
        [HttpGet]
        public async Task<List<Book>> Get() =>
            await _booksService.GetAsync();

        [HttpGet]
        [Route("/api/[controller]/login")]
        public async Task<IActionResult> Login()
        {
            ClaimsIdentity ci = new ClaimsIdentity();
            ci.AddClaim(new Claim("user", "delete"));
            ci.AddClaim(new Claim("testpolicy", "test"));
            ci.AddClaim(new Claim("testpolicy2", "test2"));
            ClaimsPrincipal cp = new ClaimsPrincipal(ci);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, cp, new
                AuthenticationProperties
            {
                //ExpiresUtc = DateTime.UtcNow.AddMinutes(9999),
            });

            return RedirectToAction(nameof(Get));
        }

        [HttpGet]
        [Route("/api/[controller]/TestPolicy")]
        [Authorize(Policy = "testpolicy2-test2")] // 可直接放置在控制上
        public async Task<IActionResult> TestPolicy()
        {
            return await Task.FromResult(Ok());
        }

        [AllowAnonymous]
        [NoLogsAttriteFilter("Manage 不需要记录访问日志")]
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            var book = await _booksService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Book newBook)
        {
            await _booksService.CreateAsync(newBook);

            return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Book updatedBook)
        {
            var book = await _booksService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            updatedBook.Id = book.Id;

            await _booksService.UpdateAsync(id, updatedBook);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var book = await _booksService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            await _booksService.RemoveAsync(id);

            return NoContent();
        }
    }
}
