using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Svistelka.Models;

namespace Svistelka.Pages
{
	public class SignModel : PageModel
	{
		ApplicationContext _context;
		[BindProperty]
		public User Person { get; set; } = new();
		public string ErrorMessage { get; set; }
		public SignModel(ApplicationContext db)
		{
			_context = db;
		}
		public void OnGet()
		{
		}
		public async Task<IActionResult> OnPostAsync()
		{
			if(Person.Password == Person.PasswordConfirmation)
            {
                _context.Users.Add(Person);
                await _context.SaveChangesAsync();
                return RedirectToPage("Auth");
            }
			else
			{
				ErrorMessage = "Пароли не совпдают";
				return Page();
			}
		}
	}
}
