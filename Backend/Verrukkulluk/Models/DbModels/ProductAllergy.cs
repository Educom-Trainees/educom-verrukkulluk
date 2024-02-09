namespace Verrukkulluk.Models.DbModels
{
    public class ProductAllergy
    {
        public int Id { get; set; }
        public int AllergyId { get; set; }
        public int ProductId { get; set; }

        public ProductAllergy() { }
        public ProductAllergy(int allergyId, int productId)
        {
            AllergyId = allergyId;
            ProductId = productId;
        }

    }
}
