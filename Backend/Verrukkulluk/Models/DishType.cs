namespace Verrukkulluk
{
    public class DishType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DishType() { }
        public DishType(string name)
        {
            Name = name;
        }
    }
    
}