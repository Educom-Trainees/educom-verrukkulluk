using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Verrukkulluk.Models;

public class ProfilePictureModel : PageModel
{
    private readonly UserManager<User> _userManager;

    public ProfilePictureModel(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public string ProfilePictureBase64 { get; set; }

    [TempData]
    public string StatusMessage { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Kies een foto uit uw bestanden door op Choose File te klikken")]
        [Display(Name = "Profielfoto")]
        [DataType(DataType.Upload)]
        public IFormFile ProfilePicture { get; set; }
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        if (user.ProfilePicture != null)
        {
            ProfilePictureBase64 = Convert.ToBase64String(user.ProfilePicture);
        }
        else
        {
            ProfilePictureBase64 = null; // or an empty string, depending on your preference
        }

        return Page();
    }

    public async Task<IActionResult> OnPostChangeProfilePictureAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Input.ProfilePicture != null)
        {
            using (var stream = Input.ProfilePicture.OpenReadStream())
            {
                using (var memoryStream = new System.IO.MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    user.ProfilePicture = memoryStream.ToArray();
                }
            }
        }

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            StatusMessage = "Uw profielfoto is bijgewerkt!";
        }
        else
        {
            StatusMessage = "Er ging iets fout tijdens het bijwerken.";
        }
        return RedirectToPage();
    }
}
