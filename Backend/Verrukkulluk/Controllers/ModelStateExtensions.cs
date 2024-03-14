using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Verrukkulluk.Controllers
{
    public static class ModelStateExtensions
    {
        public static bool IsValid(this ModelStateDictionary modelState, string key)
        {
            return modelState[key]?.ValidationState == ModelValidationState.Valid;
        }
    }
}
