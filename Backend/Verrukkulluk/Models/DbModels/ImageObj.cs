using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Verrukkulluk.Models
{
    [SwaggerSchemaFilter(typeof(ImageObjFilter))]
    public class ImageObj
    {
        /// <summary>
        /// The id of the image object
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The binary content
        /// </summary>
        public byte[] ImageContent { get; set; }
        /// <summary>
        /// The extension of the file without the dot (example "jpg", "jpeg", "png", "gif", "bmp", "tiff", "svg")
        /// </summary>
        public string ImageExtention {  get; set; }

        public ImageObj() { }
        public ImageObj(byte[] imageContent, string imageExtention) {
            ImageContent = imageContent;
            ImageExtention = imageExtention.StartsWith(".") ? imageExtention.Substring(1) : imageExtention;
        }
    }

    internal class ImageObjFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            schema.Example = new OpenApiObject
            {
                [nameof(ImageObj.Id)] = new OpenApiInteger(2),
                [nameof(ImageObj.ImageExtention)] = new OpenApiString("jpeg"),
                [nameof(ImageObj.ImageContent)] = new OpenApiString("iVBORw0KGgoAAAAN...AIU1N2TkFotH")
            };
        }
    }
}
