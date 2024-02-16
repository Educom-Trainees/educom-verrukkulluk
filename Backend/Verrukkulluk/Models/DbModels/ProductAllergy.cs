using System.Text.Json.Serialization;

namespace Verrukkulluk
{
    public class ProductAllergy
    {
        public int Id { get; set; }
        public Allergy Allergy { get; set; }
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
