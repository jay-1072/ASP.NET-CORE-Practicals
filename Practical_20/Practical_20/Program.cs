using Microsoft.EntityFrameworkCore;
using Practical_20.Data;
using Practical_20.Interfaces;
using Practical_20.Middleware;
using Serilog;

namespace Practical_20
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			//builder.Services.AddControllersWithViews();
			builder.Services.AddMvc();

			builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
			builder.Services.AddDbContext<DatabaseContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
			});

			builder.Services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

			builder.Services.AddHttpContextAccessor();

			var app = builder.Build();

			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseSerilogRequestLogging();
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseErrorHandalingMiddleware();
			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}");

			app.Run();
		}
	}
}