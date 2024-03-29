﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Verrukkulluk.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class CityOfResidenceModel : PageModel
{
    private readonly UserManager<User> _userManager;

    public CityOfResidenceModel(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public string CityOfResidence { get; set; }

    [TempData]
    public string StatusMessage { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Vul uw nieuwe woonplaats in")]
        [CustomCityNameValidation(ErrorMessage = "De woonplaats mag alleen letters bevatten, probeer opnieuw")]
        [Display(Name = "Nieuwe woonplaats")]
        public string NewCityOfResidence { get; set; }
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        CityOfResidence = user.CityOfResidence;

        return Page();
    }

    public async Task<IActionResult> OnPostChangeCityOfResidenceAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        if (!ModelState.IsValid)
        {
            CityOfResidence = user.CityOfResidence;
            return Page();
        }

        user.CityOfResidence = Input.NewCityOfResidence;
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            StatusMessage = "Uw woonplaats is aangepast!";
        }
        else
        {
            StatusMessage = "Er ging iets fout tijdens het aanpassen.";
        }
        return RedirectToPage();
    }

    public class CustomCityNameValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string cityValue = value.ToString();
                if (!Regex.IsMatch(cityValue, "^[a-zA-Z\\s]+$"))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}
