using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Api.Enroll.Interfaces;
using Api.Enroll.Repositories;
using Api.Enroll.Services;
using Polly;

namespace Api.Courses
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
            // Inject Repositories
            services.AddSingleton<IEnrollsRepository, FakeEnrollRepository>();

            // Inject Services
            services.AddSingleton<IEnrollsService, EnrollService>();
            services.AddSingleton<ICoursesService, CoursesService>();
            services.AddSingleton<IUsersService, UsersService>();
     
            // Add Http Long Polly
            services.AddHttpClient("CoursesService", config =>
            {
                config.BaseAddress = new Uri(Configuration["Services:Courses"]);
            }).AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(5, _ => TimeSpan.FromMilliseconds(500)));
            
            services.AddHttpClient("UsersService", config =>
            {
                config.BaseAddress = new Uri(Configuration["Services:Users"]);
            }).AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(5, _ => TimeSpan.FromMilliseconds(500)));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
