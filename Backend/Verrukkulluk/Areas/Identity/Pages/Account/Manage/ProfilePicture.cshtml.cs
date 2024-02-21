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
    private readonly IServicer _servicer;

    public ProfilePictureModel(UserManager<User> userManager, IServicer servicer)
    {
        _userManager = userManager;
        _servicer = servicer;
    }

    public ImageObj ProfilePicture { get; set; }

    [TempData]
    public string StatusMessage { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Kies een nieuwe profielfoto uit uw bestanden")]
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
        ProfilePicture = _servicer.GetImage(user.ImageObjId);
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
            ProfilePicture = _servicer.GetImage(user.ImageObjId);
            return Page();
        }

        if (Input.ProfilePicture != null)
        {
            using (var stream = Input.ProfilePicture.OpenReadStream())
            {
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    var imageObj = new ImageObj(memoryStream.ToArray(), Path.GetExtension(Input.ProfilePicture.FileName));
                    _servicer.SaveImage(imageObj, user);

                    user.ImageObjId = imageObj.Id;

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        StatusMessage = "Uw profielfoto is bijgewerkt!";
                    }
                    else
                    {
                        StatusMessage = "Er ging iets fout tijdens het bijwerken.";
                    }
                }
            }
            return RedirectToPage();
        }
        else
        {
            ProfilePicture = _servicer.GetImage(user.ImageObjId);
            return Page();
        }
    }
}

