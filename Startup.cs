using ExpenseApp.Models;
using ExpenseApp.Models.DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseApp
{
    public class Startup
    {
        private readonly IConfigurationRoot configuration;

        public Startup(IHostingEnvironment env)
        {
            configuration = new ConfigurationBuilder()
                .AddJsonFile(env.ContentRootPath + "/config.json")
                .AddJsonFile(env.ContentRootPath + "/config.development.json")
                .Build();
        }

        // This method gets called by the runtime. Use this method to add 
        // services to the container. For more information on how to configure 
        // your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<FeatureToggles>(sp => new FeatureToggles
            {
                EnableDeveloperExceptions =
                    configuration.GetValue<bool>("FeatureToggles:EnableDeveloperExceptions")
            });

            services.AddDbContext<EmployeeDataContext>(options =>
            {
                string connectionString = configuration
                    .GetConnectionString("EmployeeDataContext");
                options.UseSqlServer(connectionString);
            });

            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddPageRoute("/Form/Index", "");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env,
            FeatureToggles features)
        {
            if (features.EnableDeveloperExceptions)
            {
                app.UseDeveloperExceptionPage();
            }
            
            // Allow pages to be iframed by Microsoft Teams
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Content-Security-Policy",
                    "frame-ancestors teams.microsoft.com *.teams.microsoft.com *.skype.com");

                await next();
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute("Default",
                    "{controller=Form}/{action=Index}/{id?}"
                );
            });

            app.UseFileServer();
        }
    }
}