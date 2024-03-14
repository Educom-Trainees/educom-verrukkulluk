using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Verrukkulluk
{
    public class ProductAllergy
    {
        public int AllergyId { get; set; }
        [ForeignKey(nameof(AllergyId))]
        public Allergy Allergy { get; set; }
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        [JsonIgnore]
        public Product Product { get; set; }
        public ProductAllergy() { }
        public ProductAllergy(Allergy allergy, Product product)
        {
            Allergy = allergy;
            Product = product;
        }

    }
}
