using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using CurrencyRate.DataStructures;
using CurrencyRate.DataStructures.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRate
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseResponseCaching();
            app.UseMvc();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                    .AddXmlSerializerFormatters();
            services.AddResponseCaching();
            services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
            services.AddScoped<IHttpClientWrapper, HttpClientWrapper>();
            services.AddSingleton(new HttpClient());
            var endpointConfig =
                new DailyCurrencyRatesEndpointConfiguration(Configuration.GetSection("DailyCurrencyRateEndpoint")
                    .Value);
            services.AddSingleton(endpointConfig);
            services.AddScoped<IExchangeRateSl, ExchangeRateSl>();
        }
    }
}