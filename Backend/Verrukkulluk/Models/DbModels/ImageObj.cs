namespace Verrukkulluk.Models
{
    public class ImageObj
    {
        public int Id { get; set; }
        public byte[] ImageContent { get; set; }
        public string ImageExtention {  get; set; }

        public ImageObj() { }
        public ImageObj(byte[] imageContent, string imageExtention) {
            ImageContent = imageContent;
            ImageExtention = imageExtention.StartsWith(".") ? imageExtention.Substring(1) : imageExtention;
        }
    }
}
