using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using sample_ecommerce_api.Controllers;
using ServiceStack.Redis;

namespace sample_ecommerce_api
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
            services.AddTransient<IProductService, ProductService>();

            services.AddTransient<IRedisClientsManager, PooledRedisClientManager>(x => new PooledRedisClientManager(15, "localhost"));

            //HealtCheck(services.BuildServiceProvider());
        }

        private void HealtCheck(ServiceProvider serviceProvider)
        {
            serviceProvider.GetService<IRedisClientsManager>().GetClient();

            serviceProvider.GetService<IProductService>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseMvc();
        }
    }
}
