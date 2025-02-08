using Microsoft.EntityFrameworkCore;
using Svistelka.Models;

namespace Svistelka
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddRazorPages();

            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

			builder.Services.AddSession(options =>
			{
				options.Cookie.Name = "SampleSession";
				options.IdleTimeout = TimeSpan.FromMinutes(10);
				options.Cookie.IsEssential = true;
			});

			var app = builder.Build();

			if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

			app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapRazorPages();
            app.UseSession();

            app.Run();
        }
    }
}
