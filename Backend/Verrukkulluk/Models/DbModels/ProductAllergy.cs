using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Verrukkulluk
{
    public class ProductAllergy
    {
        private Allergy allergy;
        private readonly ILazyLoader _lazyLoader;

        public int AllergyId { get; set; }
        [ForeignKey(nameof(AllergyId))]
        public Allergy Allergy { get => _lazyLoader.Load(this, ref allergy); set => allergy = value; }
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        [JsonIgnore]
        public Product Product { get; set; }
        public ProductAllergy() { }
        public ProductAllergy(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }
        public ProductAllergy(Allergy allergy, Product product)
        {
            Allergy = allergy;
            Product = product;
        }

    }
}
