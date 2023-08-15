using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Calendarize.API.Capabilities
{
    public static class StartupSwagger
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            return services
                .AddSwaggerExamplesFromAssemblyOf(typeof(StartupSwagger))
                .AddSwaggerGen(options =>
                {
                    options.UseInlineDefinitionsForEnums();

                    // Automatically discover all API versions used within the application and register them as documents:
                    options.RegisterDefaultDocs(services, new OpenApiInfo
                    {
                        Title = "Calendarize.API",
                        Description = "Calendarize.API description",
                    });

                    options.ExampleFilters();
                })
                .AddSwaggerGenNewtonsoftSupport();
        }
    }

    public static class SwaggerGenOptionsExtensions
    {
        /// <summary>
        /// Registers default Swagger documents
        /// </summary>
        /// <param name="options">Options instance</param>
        /// <param name="services">Services</param>
        /// <param name="info">Information</param>
        /// <returns>Extended options</returns>
        public static SwaggerGenOptions RegisterDefaultDocs(this SwaggerGenOptions options, IServiceCollection services,
        OpenApiInfo info = null)
        {
            var provider = services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();

            if (provider == null)
                return options;

            foreach (var description in provider.ApiVersionDescriptions)
            {
                if (string.IsNullOrEmpty(description.GroupName))
                    continue;

                options.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Version = description.GroupName,
                    Title = info?.Title,
                    Description = info?.Description,
                    Contact = info?.Contact,
                    License = info?.License,
                    TermsOfService = info?.TermsOfService
                });
            }

            return options;
        }
    }
}
