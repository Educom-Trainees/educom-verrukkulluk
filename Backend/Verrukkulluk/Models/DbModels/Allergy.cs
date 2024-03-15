using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;
using Verrukkulluk.Models;

namespace Verrukkulluk
{
    [SwaggerSchemaFilter(typeof(AllergySchemaFilter))]
    public class Allergy
    {
        /// <summary>
        /// The id of the allergy
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The name of the Allergy
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// To get the image go to "/Image/GetImage/{ImgObjId}"
        /// Use &amp;lt;img src="/Image/GetImage/{ImgObjId}"&amp;gt; to show the image in html
        /// </summary>
        [Required]
        public int ImgObjId { get; set; }

        public Allergy() { }
        public Allergy(string name, int imgObjId)
        {
            Name = name;
            ImgObjId = imgObjId;
        }
    }

    internal class AllergySchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            schema.Example = new OpenApiObject
            {
                [nameof(Allergy.Id)] = new OpenApiInteger(2),
                [nameof(Allergy.Name)] = new OpenApiString("Nuts"),
                [nameof(Allergy.ImgObjId)] = new OpenApiInteger(57)
            };
        }
    }
}