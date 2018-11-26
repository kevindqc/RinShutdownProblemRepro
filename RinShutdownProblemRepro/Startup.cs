using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace RinShutdownProblemRepro
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
            services
                .AddMvc()
                .AddRinMvcSupport()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddRin();
        }

        //public void ConfigureContainer(ContainerBuilder builder)
        //{

        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Add: Enable request/response recording and serve a inspector frontend.
                // Important: `UseRin` (Middlewares) must be top of the HTTP pipeline.
                app.UseRin();

                app.UseRinMvcSupport();

                app.UseDeveloperExceptionPage();

                // Add: Enable Exception recorder. this handler must be after `UseDeveloperExceptionPage`.
                app.UseRinDiagnosticsHandler();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
