namespace Verrukkulluk
{
    public class KitchenType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public KitchenType() { }
        public KitchenType(string name)
        {
            Name = name;
        }
    }
    
}