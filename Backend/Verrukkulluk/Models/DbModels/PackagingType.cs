namespace Verrukkulluk.Models
{
    public class PackagingType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PackagingType() { }
        public PackagingType(string name)
        {
            Name = name;
        }
    }
}
