using DutchTreat.Data;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DutchTreat
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<DutchContext>(cfg => {
				cfg.UseSqlServer(Configuration.GetConnectionString("DutchConnectionString"));
			});

			services.AddTransient<IMailService, NullMailService>();

			services.AddTransient<DutchSeeder>();

			services.AddAutoMapper(Assembly.GetExecutingAssembly());

			services.AddScoped<IDutchRepository, DutchRepository>();

			services.AddMvc();

			services.AddControllersWithViews()
				.AddRazorRuntimeCompilation()
				.AddNewtonsoftJson(cfg => cfg.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


			services.AddRazorPages();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/error");
			}
			
			//app.UseDefaultFiles();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseEndpoints(cfg =>
			{
				cfg.MapRazorPages();
				cfg.MapControllerRoute("Default",
					"/{controller}/{action}/{id?}",
					new { controller = "App", action = "Index" });
			});

			#region OldCommented
			//if (env.IsDevelopment())
			//{
			//	app.UseDeveloperExceptionPage();
			//}
			//else
			//{
			//	app.UseExceptionHandler("/Error");
			//}

			//app.UseStaticFiles();

			//app.UseRouting();

			//app.UseAuthorization();

			//app.UseEndpoints(endpoints =>
			//{
			//	endpoints.MapRazorPages();
			//});
			#endregion

		}
	}
}
