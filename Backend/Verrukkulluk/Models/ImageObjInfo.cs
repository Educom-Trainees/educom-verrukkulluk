using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Verrukkulluk.Models
{
    [SwaggerSchemaFilter(typeof(ImageObjInfoFilter))]
    public class ImageObjInfo
    {
        public int Id { get; set; }
        public EImageObjType UsedBy { get; set; }
    }

    internal class ImageObjInfoFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            schema.Example = new OpenApiObject
                {
                    [nameof(ImageObjInfo.Id)] = new OpenApiInteger(61),
                    [nameof(ImageObjInfo.UsedBy)] = new OpenApiString(EImageObjType.Allergy.ToString())
                };
        }
    }
}
