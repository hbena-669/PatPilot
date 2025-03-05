using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PatPilot.Models;
using PatPilot.Models.DTO;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace PatPilot.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginModel(SignInManager<ApplicationUser> signInManage, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManage;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Nom d'utilisateur")]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Mot de passe")]
            public string Password { get; set; }

            [Display(Name = "Se souvenir de moi")]
            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.Users
                                        .Include(u => u.Enseigne) // Charge l'objet Enseigne
                                        .FirstOrDefaultAsync(u => u.UserName == Input.UserName);

                    if (user != null && user.Enseigne != null)
                    {
                        var enseigneDTO = new EnseigneDTO
                        {
                            Id = user.Enseigne.Id,
                            Name = user.Enseigne.Name,
                            Logo = user.Enseigne.logo
                        };

                        // Convertir l'objet Enseigne en JSON pour la session
                        var enseigneJson = JsonSerializer.Serialize(enseigneDTO);
                        HttpContext.Session.SetString("Enseigne", enseigneJson);
                        HttpContext.Session.SetString("EnseigneId", enseigneDTO.Id.ToString());
                        HttpContext.Session.SetString("EnseigneName", enseigneDTO.Name);
                    }
                    return LocalRedirect("/Home/index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tentative de connexion invalide.");
                    return Page();
                }
            }

            // Si nous arrivons ici, quelque chose a échoué
            return Page();
        }
    }
}
