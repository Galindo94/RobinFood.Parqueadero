

namespace Applicacion.Transferencias.WebApi.Handlers
{
    using Common.Utils.Swagger;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Microsoft.Extensions.DependencyInjection;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    [ExcludeFromCodeCoverage]
    public static class SwaggerHandler
    {
        public static void SwaggerConfig(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.IncludeXmlComments(Path.ChangeExtension(typeof(Startup).Assembly.Location, "xml"));

                options.SwaggerDoc(string.Format("v{0}", SwaggerConfiguration.DocInfoVersion), new Info
                {
                    Version = SwaggerConfiguration.DocInfoVersion,
                    Title = SwaggerConfiguration.DocInfoTitle,
                    Description = SwaggerConfiguration.DocInfoDescription
                });

                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var actionApiVersionModel = (ApiVersionModel)apiDesc.ActionDescriptor?.Properties
                        .FirstOrDefault(w => ((Type)w.Key).Equals(typeof(ApiVersionModel))).Value;
                    if (actionApiVersionModel == null)
                    {
                        return true;
                    }
                    return actionApiVersionModel.DeclaredApiVersions.Any()
                        ? actionApiVersionModel.DeclaredApiVersions.Any(version => $"v{version.ToString()}".Equals(docName))
                        : actionApiVersionModel.ImplementedApiVersions.Any(version => $"v{version.ToString()}".Equals(docName));
                });

                options.OperationFilter<RemoveVersionParameterFilter>();
                options.DocumentFilter<SetVersionInPathFilter>();

                options.CustomSchemaIds(x => x.FullName);

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", Array.Empty<string>() }
                };

                options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                options.AddSecurityRequirement(security);
            });

            //services.AddSwaggerGen(options =>
            //{
            //    options.DescribeAllEnumsAsStrings();
            //    options.IncludeXmlComments(Path.ChangeExtension(typeof(Startup).Assembly.Location, "xml"));
            //    options.SwaggerDoc(SwaggerConfiguration.DocInfoVersion, new Info
            //    {
            //        Version = SwaggerConfiguration.DocInfoVersion,
            //        Title = SwaggerConfiguration.DocInfoTitle,
            //        Description = SwaggerConfiguration.DocInfoDescription
            //    });
            //    options.CustomSchemaIds(x => x.FullName);

            //    var security = new Dictionary<string, IEnumerable<string>>
            //    {
            //            { "Bearer", Array.Empty<string>() }
            //    };

            //    options.AddSecurityDefinition("Bearer", new ApiKeyScheme
            //    {
            //        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            //        Name = "Authorization",
            //        In = "header",
            //        Type = "apiKey"
            //    });

            //    options.AddSecurityRequirement(security);
            //});
        }

        public static void UseSwagger(IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1.0/swagger.json", "Parqueadero Web Api");
            });
        }
    }

    [ExcludeFromCodeCoverage]
    public class RemoveVersionParameterFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext operationFilterContext)
        {
            var parameterVersion = operation?.Parameters?.SingleOrDefault(param => param.Name.Equals("version"));
            operation?.Parameters?.Remove(parameterVersion);
        }
    }

    [ExcludeFromCodeCoverage]
    public class SetVersionInPathFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            //Nos recorremeros las ditintas URL que nos llegan en el documento de swagger y reemplazaremos
            //la versión que nos llega en la información del documento.
            swaggerDoc.Paths = swaggerDoc.Paths
                .ToDictionary(
                    path => path.Key.Replace("{version}", swaggerDoc.Info.Version),
                    path => path.Value
                );
        }
    }
}