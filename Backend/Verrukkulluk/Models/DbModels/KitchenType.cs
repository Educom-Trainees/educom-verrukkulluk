namespace Verrukkulluk
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;

    public class KitchenType
    {
        public const string Other = "Overig";
        
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; } = true;

        public KitchenType() { }
        public KitchenType(string name)
        {
            Name = name;
        }
    }
    
}