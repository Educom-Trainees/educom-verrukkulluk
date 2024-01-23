using Verrukkulluk.Data;

namespace Verrukkulluk.Models
{
    public class VerModel
    {
        private readonly ICrud Crud;
        public VerModel(ICrud crud)
        {
            Crud = crud;
        }
    }
}
