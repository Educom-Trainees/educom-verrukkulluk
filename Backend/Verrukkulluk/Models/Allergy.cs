using Verrukkulluk.Models;

namespace Verrukkulluk
{
    public class Allergy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Allergy> Allergies { get; set; }

        public Allergy() { }
        public Allergy(string name)
        {
            Name = name;
        }
    }
    
}