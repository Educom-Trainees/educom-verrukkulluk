// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Verrukkulluk.Models;

namespace Verrukkulluk.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IServicer _servicer;

        public RegisterModel(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, 
            IServicer servicer)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _servicer = servicer;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "Vul uw voornaam in")]
            [Display(Name = "Voornaam")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Vul uw woonplaats in")]
            [Display(Name = "Woonplaats")]
            public string CityOfResidence { get; set; }

            [Required(ErrorMessage = "Vul uw e-mail in")]
            [EmailAddress]
            [Display(Name = "E-mail")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Selecteer uw profielfoto")]
            [Display(Name = "Profielfoto")]
            public IFormFile ProfilePicture { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "Vul uw wachtwoord in")]
            [StringLength(100, ErrorMessage = "Het {0} moet minstens {2} en maximaal {1} tekens lang zijn.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Wachtwoord")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Herhaal wachtwoord")]
            [Compare("Password", ErrorMessage = "De wachtwoorden komen niet overeen.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                if (Input.ProfilePicture == null || Input.ProfilePicture.Length == 0)
                {
                    ModelState.AddModelError("Input.ProfilePicture", "Selecteer uw profielfoto");
                    return Page();
                }

                var user = CreateUser();

                using (var memoryStream = new MemoryStream())
                {
                    await Input.ProfilePicture.CopyToAsync(memoryStream);
                    var imageObj = new ImageObj(memoryStream.ToArray(), Path.GetExtension(Input.ProfilePicture.FileName));
                    _servicer.SaveImage(imageObj, user);
                    user.ImageObjId = imageObj.Id;
                }

                user.CityOfResidence = Input.CityOfResidence;
                user.FirstName = Input.FirstName;

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("U bent succesvol geregisteerd.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Bevestig alstublieft uw account <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>klik hier</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect("/Identity/Account/Manage");
                    }
                }
                foreach (var error in result.Errors)
                {
                    if (error.Code == "DuplicateUserName")
                    {
                        ModelState.AddModelError(string.Empty, "Dit e-mailadres bestaat al. Kies een andere.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        var errorMessage = error.ErrorMessage;
                        var exceptionMessage = error.Exception?.Message;
                        _logger.LogError(errorMessage);
                        if (exceptionMessage != null)
                        {
                            _logger.LogError(exceptionMessage);
                        }
                    }
                }
                if (Input.ProfilePicture == null || Input.ProfilePicture.Length == 0)
                {
                    ModelState.AddModelError("Input.ProfilePicture", "Selecteer uw profielfoto");
                }
            }
            return Page();
        }

        private User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
                    $"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<User>)_userStore;
        }
    }
}
