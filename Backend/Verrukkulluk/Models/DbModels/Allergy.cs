using Verrukkulluk.Models;

namespace Verrukkulluk
{
    public class Allergy
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int ImgObjId { get; set; }

        public Allergy() { }
        public Allergy(string name, int imgObjId)
        {
            Name = name;
            ImgObjId = imgObjId;
        }
    }
    
}