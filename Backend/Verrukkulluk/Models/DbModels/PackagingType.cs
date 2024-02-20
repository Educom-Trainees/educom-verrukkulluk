namespace Verrukkulluk.Models
{
    public class PackagingType
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public PackagingType() { }
        public PackagingType(int typeId, string name)
        {
            TypeId = typeId;
            Name = name;
        }
    }
}
