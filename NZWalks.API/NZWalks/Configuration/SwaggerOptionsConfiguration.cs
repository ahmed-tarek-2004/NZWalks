using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NZWalks.API.Configurations
{
    public class SwaggerOptionsConfiguration : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

        public SwaggerOptionsConfiguration(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            _apiVersionDescriptionProvider = apiVersionDescriptionProvider;
        }
        public void Configure(string? name, SwaggerGenOptions options)
        {
            if (name == Options.DefaultName)
            {
                Configure(options);
            }
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc($"v{description.ApiVersion}", CreateVersionInfo(description));

            }
        }

        private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = $"v{description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = "API documentation for versioned endpoints"
            };

            if (description.IsDeprecated)
            {
                info.Description += " (This version is deprecated)";
            }

            return info;
        }
    }
}