using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMemoryTesting.Configuration;
using InMemoryTesting.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace inmemorytesting
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
            ConfigureDatabases(services);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if(env.IsEnvironment("Testing") == false)
            {
                using(var scope = app.ApplicationServices.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<MovieContext>();
                    context.Database.EnsureCreated();
                }
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        protected virtual void ConfigureDatabases(IServiceCollection services)
        {
            // var sqlite = new SqliteConnection("Data Source=file:memdb?mode=memory&cache=shared");
            // sqlite.Open();

            // services.Configure<DatabaseConfiguration>(c => 
            //     c.RootConnectionString = "Data Source=file:memdb?mode=memory&cache=shared"
            // );

            // services.AddDbContext<MovieContext>(options => options.UseSqlite(sqlite));
        }
    }
}
