using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Svistelka.Models;

namespace Svistelka.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<ProfileModel> _log;

        public User ProfileUser { get; set; }
        public User CurrentUser { get; set; }

        public bool IsFollow { get; set; }

        public ProfileModel(ILogger<ProfileModel> log, ApplicationContext context)
        {
            _log = log;
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync([FromRoute] int id)
        {
            ProfileUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (ProfileUser == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            var sessionId = HttpContext.Session.GetString("SampleSession");
            if (string.IsNullOrEmpty(sessionId))
            {
                return RedirectToPage("Auth");
            }

            CurrentUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(sessionId));

            IsFollow = await _context.Relations.AnyAsync(r => r.FollowerId == int.Parse(sessionId) && r.FollowedId == ProfileUser.Id);

            return Page();
        }

        // Подписаться на пользователя
            public async Task<IActionResult> OnPostFollowAsync([FromRoute] int id)
            {
                var sessionId = HttpContext.Session.GetString("SampleSession");
                if (string.IsNullOrEmpty(sessionId))
                {
                    return RedirectToPage("Auth");
                }

                ProfileUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (ProfileUser == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                CurrentUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(sessionId));

                IsFollow = await _context.Relations.AnyAsync(r => r.FollowerId == int.Parse(sessionId) && r.FollowedId == ProfileUser.Id);
            
                if(!IsFollow)
                {
                    try
                    {
                        var relation = new Relation()
                        {
                            FollowerId = CurrentUser.Id,
                            FollowedId = ProfileUser.Id
                        };

                        _context.Relations.Add(relation);
                        await _context.SaveChangesAsync();

                        _log.LogInformation($"User {CurrentUser.Name} followed {ProfileUser.Name}.");
                        IsFollow = true;
                    }
                    catch (Exception ex)
                    {
                        _log.LogError(ex, $"Error following user {ProfileUser.Name}.");
                    }
                }

                else
                {
                    try
                    {
                        var relation = await _context.Relations.FirstOrDefaultAsync(r =>
                          r.FollowerId == CurrentUser.Id && r.FollowedId == ProfileUser.Id);

                        if (relation != null)
                        {
                            _context.Relations.Remove(relation);
                            await _context.SaveChangesAsync();

                            _log.LogInformation($"User {CurrentUser.Name} unfollowed {ProfileUser.Name}.");
                            IsFollow = false;
                        }
                        else
                        {
                            _log.LogWarning($"Relation not found for unfollowing {ProfileUser.Name} by {CurrentUser.Name}.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.LogError(ex, $"Error unfollowing user {ProfileUser.Name}.");
                    }
                }

                return Page();
            }
    }
}
