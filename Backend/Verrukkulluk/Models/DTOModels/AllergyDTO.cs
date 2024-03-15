using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Verrukkulluk.Models.DTOModels
{
    [SwaggerSchemaFilter(typeof(AllergySchemaFilter))]
    public class AllergyDTO
    {
        /// <summary>
        /// The id of the allergy
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// The name of the Allergy
        /// </summary>
        /// <remarks>May be empty on PUT/POST</remarks>
        public string? Name { get; set; }
        /// <summary>
        /// To get the image go to "/Image/GetImage/{ImgObjId}"
        /// Use &lt;img src="/Image/GetImage/{ImgObjId}"&gt; to show the image in html
        /// </summary>
        /// <remarks>May be empty on PUT/POST</remarks>
        public int? ImgObjId { get; set; }
    }
}