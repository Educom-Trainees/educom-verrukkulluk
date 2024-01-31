using Microsoft.AspNetCore.Identity;

namespace Verrukkulluk.Models
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public List<Ingredient> ShoppingList { get; set; }
        public List<Recipe> FavouritesList { get; set; }
        public string CityOfResidence { get; set; }

        public User() { }

        public byte[]? ProfilePicture { get; set; }
        public User(string email, string firstName, string cityOfResidence, byte[]? newPicture)
        {
            Email = email;
            FirstName = firstName;
            UserName = firstName;
            CityOfResidence = cityOfResidence;
            ProfilePicture = newPicture;
        }
        public static byte[] ReadImageFile(string fileName)
        {
            string folderPath = "wwwroot/Images";
            string filePath = Path.Combine(folderPath, fileName);

            try
            {
                if (File.Exists(filePath))
                {
                    return File.ReadAllBytes(filePath);
                }
                else
                {
                    Console.WriteLine($"File not found: {filePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }

            return null;
        }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
