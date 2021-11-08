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
using System.Reflection;
using System.Threading.Tasks;
using API.Attributes;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Domain.Entities;
using Domain.Mappings;
using Domain.UnitOfWork;
using Infrastructure;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment CurrentEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v3" });
                c.AddSecurityDefinition("emailAuth", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Cookie,
                    Name = "email"
                });
                c.AddSecurityDefinition("userIdAuth", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Cookie,
                    Name = "userId"
                });
            });

            services.AddMvcCore(options =>
            {
                options.Filters.Add(new CookieMapGlobalAttribute());
            });

            var builder = new ContainerBuilder();
            builder.RegisterModule(new NHibernateModule()
            {
                DbType = "SqlServer",
                ConnectionString = Configuration.GetConnectionString("LOCAL"),
                MappingAssemblies = new List<Assembly>()
                {
                    typeof(AccountDeletionRequestMap).Assembly
                }
            });

            builder.RegisterModule(new RepositoryModule()
            {
                RepositoryAssemblies = new System.Collections.Generic.List<Assembly>()
                {
                    typeof(AccountDeletionRequest).Assembly
                }
            });

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.Populate(services);

            var container = builder.Build();

            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
