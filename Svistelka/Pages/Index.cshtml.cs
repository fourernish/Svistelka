using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Svistelka.Models;

namespace Svistelka.Pages
{
    public class IndexModel : PageModel
    {
        private ApplicationContext _context;
        private readonly ILogger<IndexModel> _logger;

        public User? user;
        public string sessionId { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
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
                user = await _context.Users.FirstOrDefaultAsync(m => m.Id == int.Parse(sessionId));
                if(user == null) 
                    return NotFound();
                return Page();
			}
		}
    }
}
