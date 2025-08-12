using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

public class SwaggerOptionsConfiguration : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider provider;

    public SwaggerOptionsConfiguration(IApiVersionDescriptionProvider provider)
    {
        this.provider = provider;
    }

    public void Configure(string name, SwaggerGenOptions options) => Configure(options);

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Title = $"NZWalks API {description.ApiVersion}",
                Version = description.ApiVersion.ToString()
            });
        }
    }
}
