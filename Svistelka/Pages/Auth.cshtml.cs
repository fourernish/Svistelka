using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Svistelka.Models;

namespace Svistelka.Pages
{
    public class AuthModel : PageModel
    {
        ApplicationContext _context;
		[BindProperty]
		public User Person { get; set; } = new();
        public AuthModel(ApplicationContext db)
        {
			_context = db;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            _context.Users.Where(u => u.Email == Person.Email && u.Password == Person.Password);
            return RedirectToPage("Index");

		}
    }
}
