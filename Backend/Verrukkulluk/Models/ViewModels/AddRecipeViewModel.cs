using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;
using Verrukkulluk.Models;

namespace Verrukkulluk.ViewModels
{
    public class AddRecipeViewModel : AddRecipe
    {
        public List<SelectListItem> MyKitchenTypeOptions { get; set; } = new List<SelectListItem>() { 
            new SelectListItem() { Text = "- Type keuken -", Value = "0", Selected = true, Disabled = true } 
        };
    }
}