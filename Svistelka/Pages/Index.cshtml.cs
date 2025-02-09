using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Svistelka.Models;
using Microsoft.AspNetCore.Http;
using static System.Net.WebRequestMethods;

namespace Svistelka.Pages
{
	public class IndexModel : PageModel
	{
		private ApplicationContext _context;
		private readonly ILogger<IndexModel> _logger;

		public User? User;
		public string sessionId { get; set; }
		public List<User> Users { get; set; } = new();
		public List<Micropost> Messages { get; set; } = new();
		public IEnumerable<User> Followeds { get; set; }


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
				User = await _context.Users.FirstOrDefaultAsync(m => m.Id == int.Parse(sessionId));
				if(User == null)
				{
					HttpContext.Session.Remove("SampleSession");
					return NotFound();
				}

				Followeds = User.RelationFollowers.Select(item => item.Followed).ToList();

                Users.AddRange(Followeds);
                Users.Add(User);

				foreach(var u in Users)
				{
					var messages = await _context.Microposts.Where(m => m.UserId == u.Id).ToListAsync();

					Messages.AddRange(messages);
				}

                return Page();
			}
		}
		public async Task<IActionResult> OnPostAsync(string message)
		{
			sessionId = HttpContext.Session.GetString("SampleSession");
			User = await _context.Users.FirstOrDefaultAsync(m => m.Id == int.Parse(sessionId));

			if(!string.IsNullOrWhiteSpace(message))
			{
				var m = new Micropost()
				{
					Content = message,
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow,
					UserId = User.Id
				};

				try
				{
					await _context.Microposts.AddAsync(m);
					await _context.SaveChangesAsync();
					return RedirectToPage();
				}
				catch (Exception ex)
				{
					_logger.Log(LogLevel.Error, $"Ошибка создания сообщения: {ex.InnerException}");
					return Page();
				}
			}
			else
			{
				return Page();
			}
		}

		public async Task<IActionResult> OnGetDeleteAsync([FromQuery] int id)
		{
			sessionId = HttpContext.Session.GetString("SampleSession");
			User = _context.Users.FirstOrDefault(m => m.Id == int.Parse(sessionId));

			try
			{
				var message = await _context.Microposts.FindAsync(id);

				if (message == null)
				{
					return NotFound("Message not found");
				}

				_context.Microposts.Remove(message);
				await _context.SaveChangesAsync();

				return RedirectToPage();
			}
			catch (Exception ex)
			{
				_logger.Log(LogLevel.Error, $"Ошибка удаления сообщения: {ex.InnerException}");
				_logger.Log(LogLevel.Error, $"Модель привязки из маршрута: {id}");
			}

			return Page();
		}
	}
}
