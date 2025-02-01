using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Svistelka.Models;
using Microsoft.AspNetCore.Http;

namespace Svistelka.Pages
{
    public class IndexModel : PageModel
    {
        private ApplicationContext _context;
        private readonly ILogger<IndexModel> _logger;

        public User? Person;
        public string sessionId { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

		public async Task<IActionResult> OnGetAsync()
        {
			sessionId = HttpContext.Session.GetString("SampleSession");

            if (sessionId == null)
            {
                return RedirectToPage("Auth");
            }
            else
            {
                Person = await _context.Users.FirstOrDefaultAsync(m => m.Id == int.Parse(sessionId));
                if(Person == null)
                {
                    HttpContext.Session.Remove("SampleSession");
                    return NotFound();
                }
                    
                return Page();
			}
		}
    }
}
