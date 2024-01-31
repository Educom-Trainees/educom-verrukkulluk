using System.Drawing;

namespace Verrukkulluk.Models
{
    public class Utils
    {
        public static Image GetImageFromRecipeId(byte[] byteArray)
        {
            using (var ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }
    }
}
