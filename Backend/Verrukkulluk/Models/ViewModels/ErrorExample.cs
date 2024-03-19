using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Verrukkulluk.Models.ViewModels
{
    [SwaggerSchemaFilter(typeof(ErrorExampleFilter))]
    public class ErrorExample
    {
        public string[] Field1 { get; set; }
        public string[]? Field2 { get; set; }
    }

    internal class ErrorExampleFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            schema.Example = new OpenApiObject
            {
                ["{field1}"] = new OpenApiArray
                {
                    new OpenApiString("Error 1"),
                    new OpenApiString("Error 2")
                },
                ["{field2}"] = new OpenApiArray
                {
                    new OpenApiString("Field is missing")
                }
            };
        }
    }
}
