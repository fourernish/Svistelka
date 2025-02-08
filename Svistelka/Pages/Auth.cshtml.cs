using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Svistelka.Models;
using Microsoft.AspNetCore.Http;

namespace Svistelka.Pages
{
    public class AuthModel : PageModel
    {
        ApplicationContext _context;
        private readonly ILogger<AuthModel> _logger;
		[BindProperty]
		public User Person { get; set; } = new();
        public string ErrorMessage { get; set; }
        public AuthModel(ApplicationContext db, ILogger<AuthModel> logger)
        {
			_context = db;
            _logger = logger;
        }
        public async Task<IActionResult> OnPostAsync()
        {
			var user = await _context.Users.Where(u => u.Email == Person.Email && u.Password == Person.Password).ToListAsync();

            if (user.Count > 0)
			{
				HttpContext.Session.SetString("SampleSession", $"{user[0].Id.ToString()}");
				return RedirectToPage("/Index");
			}
            else
            {
                ErrorMessage = "Неверный логин или пароль";
                return Page();
            }
		}
    }
}
