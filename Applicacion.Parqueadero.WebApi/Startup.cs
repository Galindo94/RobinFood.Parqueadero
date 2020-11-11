namespace Applicacion.Transferencias.WebApi
{
    using Applicacion.Transferencias.WebApi.Handlers;
    using Common.Utils.JWT;
    using Common.Utils.Swagger;
    using Common.Utils.Utils.Interface;
    using Infraestructure.Core.Context;
    using Infraestructure.Core.Repository;
    using Infraestructure.Core.Repository.Interface;
    using Infraestructure.Core.UnitOfWork;
    using Infraestructure.Core.UnitOfWork.Interface;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.AspNetCore.Mvc.Formatters.Xml;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Quartz;
    using Quartz.Impl;
    using Quartz.Spi;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerUI;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Text;

    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(env.ContentRootPath)
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
           .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        //services.AddMvc().AddXmlSerializerFormatters();
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            IConfigurationSection jwtAppSettings = Configuration.GetSection("Jwt");
            services.Configure<JwtSetting>(jwtAppSettings);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    p => p.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddDistributedMemoryCache();
            services.AddSession();

            #region Swagger Configuration

            SwaggerHandler.SwaggerConfig(services);
            services.AddApiVersioning();

            #endregion Swagger Configuration

            //#region Jwt Configuration

            //JwtConfigurationHandler.ConfigureJwtAuthentication(services, jwtAppSettings);

            //#endregion Jwt Configuration

            #region Context SQL Server

            services.AddDbContext<Infraestructure.Core.Context.SQLServer.ContextSql>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("ConnectionStringSQLServer"),
               providerOptions => providerOptions.EnableRetryOnFailure()));

            #endregion Context SQL Server

            #region Register (dependency injection)

            DependencyInyectionHandler.DependencyInyectionConfig(services);

            #endregion Register (dependency injection)

            #region Job

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            var cultureInfo = new CultureInfo("es-CO");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseSession();
            SwaggerHandler.UseSwagger(app);
            app.UseDeveloperExceptionPage();
            app.UseMvc();

            appLifetime.ApplicationStarted.Register(() =>
            {
            });

            appLifetime.ApplicationStopped.Register(() =>
            {
            });
            
        }
    }
}