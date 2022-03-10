using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServerManagement.Client.Web.Configuration;
using ServerManagement.Utilities.Log4NetDetection;
using ServerManagement.Utilities.LogDetection;

namespace ServerManagement.Client.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration.GetSection("GetServiceSettings").Get<GetServiceSettings>());
            services.AddSingleton<ILogDetector, Log4NetDetector>();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddMediatR(typeof(Core.Services.Queries.GetServices.GetServicesRequestHandler));
            services.AddMediatR(typeof(Core.Services.Queries.GetServiceLogs.HasServiceLogsRequestHandler));
            services.AddMediatR(typeof(Core.Services.Queries.GetServiceLogs.GetServiceLogsRequestHandler));
            services.AddMediatR(typeof(Core.IIS.Queries.GetApplicationPools.GetApplicationPoolRequestHandler));
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
