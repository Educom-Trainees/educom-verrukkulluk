namespace Verrukkulluk.Models
{
    public class ImageObjInfo
    {
        public int Id { get; set; }
        public EImageObjType UsedBy { get; set; }
    }

    public enum EImageObjType
    {
        None,
        Allergy,
        Product,
        Recipe,
        User
    }
}
