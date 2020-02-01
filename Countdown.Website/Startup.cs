using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Countdown.NumbersRound.Solve;
using Countdown.Website.Caching;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
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

            services.AddMemoryCache();
            services.AddSingleton<IDelegateCache, DelegateMemoryCache>();
            services.AddTransient<ISolver, Solver>();

            services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new Info { Title = "Countdown Solve API", Version = "v1" })
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ISolver solver)
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

            PopulateCache(solver);
        }

        private static void PopulateCache(ISolver solver)
        {
            solver.GetPossibleSolutions(1365, new List<int> { 1, 1, 5, 7, 10, 10 });
        }
    }
}
