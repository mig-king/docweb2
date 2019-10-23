using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Rewrite;
using EnsureThat;
using fileweb.Models;
using fileweb.Models.SqliteImpl;

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

            services.AddSingleton<IDocAccessor>(new SqliteDocAccessor(Configuration.GetValue<string>("Cms_Db_ConnectionString")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDocAccessor docAccessor)
        {
            var cat1Task = docAccessor.GetAllCategory1();

            cat1Task.Wait();

            var cat1 = cat1Task.Result;

            if (cat1 != null)
            {
                var options = new RewriteOptions()
                    .AddRewrite($"^$", $"docs/", skipRemainingRules: true);

                foreach (var c in cat1)
                    options = options.AddRewrite($"(?i)^{c}", $"docs/{c}", skipRemainingRules: true);

                app.UseRewriter(options);
            }

            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
