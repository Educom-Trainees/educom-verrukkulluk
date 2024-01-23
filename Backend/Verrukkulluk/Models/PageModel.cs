using Verrukkulluk.Data;

namespace Verrukkulluk.Models
{
    public class PageModel
    {
        private readonly ICrud Crud;
        public PageModel(ICrud crud)
        {
            Crud = crud;
        }
    }
}
