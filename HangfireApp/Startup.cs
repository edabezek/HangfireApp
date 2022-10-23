using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireApp
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HangfireApp", Version = "v1" });
            });
            //To be able to use our new service with dependency injection, inside the ConfigureServices() method we register our interface implementation using the AddScoped() method.
            services.AddScoped<IJobTestService, JobTestService>();  
            services.AddHangfire(x =>
            {
                x.UseSqlServerStorage(Configuration.GetConnectionString("DBConnection"));
            });
            services.AddHangfireServer();//Hangfire server in our project is going to live inside the application.
        }

        //we have to uptade request processing pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HangfireApp v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            //alternatif dashboard
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //we can easly monitor our jobs
            app.UseHangfireDashboard();
            //This dashboard is, by default, configured to be accessible only locally, but if you want to access it remotely, you can configure that as well. However, it exposes a lot of sensitive information, so always be careful with that configuration.
        }
    }
}
