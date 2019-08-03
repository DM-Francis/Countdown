using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Countdown.NumbersRound;
using Countdown.NumbersRound.ExpressionBased;
using Countdown.Website.DataModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Countdown.Website
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
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddRazorPagesOptions(options =>
                    options.Conventions
                        .AddPageRoute("/NumbersRound", "/")
                        .AddPageRoute("/NumbersRound", "/Index")
                )
                .ConfigureApiBehaviorOptions(options =>
                    options.SuppressInferBindingSourcesForParameters = true
                );

            services.AddDbContext<CountdownContext>(options => options.UseSqlite(Configuration.GetConnectionString("Default")));
            services.AddTransient<ISolver, ExpressionSolver>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = _ => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new Info { Title = "Countdown Solve API", Version = "v1" })
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint("./v1/swagger.json", "Countdown Solve API v1")
            );

            app.UseMvc();
        }
    }
}
