using fileweb.Middlewares;
using fileweb.Models;
using fileweb.Models.SqlServer;
using fileweb.Repositories;
using fileweb.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace fileweb
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<EnvOptions>(Configuration.GetSection("EnvOptions"));

            services.AddSingleton<IDocAccessor>(new SqlServerDocAccessor(Configuration.GetValue<string>("Cms_Db_ConnectionString")));
            services.AddSingleton<IAnnouncementRepository>(new AnnouncementRepository(Configuration.GetValue<string>("Cms_Db_ConnectionString")));

            services.AddScoped<IAnnouncementService, AnnouncementService>();
            services.AddSession(options =>
            {
                options.Cookie.Name = Configuration["EnvOptions:PortalUserSessionOptions:PortalUserSessionCookieName"];
                options.IdleTimeout = TimeSpan.FromMinutes(Convert.ToDouble(Configuration["EnvOptions:PortalUserSessionOptions:SessionExpiredMinutes"]));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDocAccessor docAccessor, IOptionsSnapshot<EnvOptions> optionsAccessor)
        {
            app.UseStaticFiles();
            app.UseSession();
            //app.UsePortalUserMiddleware(optionsAccessor.Value.PortalUrl, optionsAccessor.Value);
            app.UseMvc();
        }
    }
}
