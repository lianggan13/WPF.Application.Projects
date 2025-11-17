using Microsoft.AspNetCore.Authentication;
using YunDa.ASIS.Server.Models;

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

        [HttpGet]
        public async Task<List<Book>> Get()
        {
            throw new Exception("我的异常");
            return await _booksService.GetAsync();
        }

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
