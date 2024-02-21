using System.Collections.Generic;
using Verrukkulluk.Models;

namespace Verrukkulluk.ViewModels
{
    public class AdminModel
    {
        public List<User> Users { get; set; }
        public List<RecipeInfo> Recipes { get; set; }
    }
}