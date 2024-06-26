﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Verrukkulluk.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using static CityOfResidenceModel;

public partial class FirstNameModel : PageModel
{
    private readonly UserManager<User> _userManager;

    public FirstNameModel(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public string FirstName { get; set; }

    [TempData]
    public string StatusMessage { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Vul uw correcte voornaam in")]
        [CustomCityNameValidation(ErrorMessage = "Uw voornaam mag alleen letters bevatten, probeer opnieuw")]
        [Display(Name = "Voornaam")]
        public string FirstName { get; set; } = "";
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        FirstName = user.FirstName;

        return Page();
    }

    public async Task<IActionResult> OnPostChangeFirstNameAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        if (!ModelState.IsValid)
        {
            FirstName = user.FirstName;
            return Page();
        }

        user.FirstName = Input.FirstName;
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            StatusMessage = "Uw voornaam is aangepast!";
        }
        else
        {
            StatusMessage = "Er ging iets fout tijdens het aanpassen.";
        }
        return RedirectToPage();
    }
    public partial class CustomFirstNameValidationAttribute : ValidationAttribute
    {
        [GeneratedRegex("^[a-zA-Z\\s]+$")]
        private static partial Regex FirstNameRegex();

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("First name is missing");
            }
            string nameValue = value.ToString() ?? "";
            if (!FirstNameRegex().IsMatch(nameValue))
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }

    }
}
