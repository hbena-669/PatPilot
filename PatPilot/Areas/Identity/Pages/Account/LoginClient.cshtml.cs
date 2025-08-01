using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PatPilot.Data;
using PatPilot.Models;
using PatPilot.Models.DTO;
using PatPilot.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PatPilot.Areas.Identity.Pages.Account
{

    public class LoginClientModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public LoginClientModel(ApplicationDbContext context, SignInManager<ApplicationUser> signInManage, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManage;
            _userManager = userManager;
            _context = context;
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
                        //sauvegarde du client
                        var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == user.Id);
                        if(client != null)
                        {
                            HttpContext.Session.SetString(Constantes.S_client_id_client, client.Id.ToString());
                            var options = new JsonSerializerOptions
                            {
                                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                                WriteIndented = true
                            };
                            HttpContext.Session.SetString(Constantes.S_client_client, JsonSerializer.Serialize(client,options));
                        }
                    }
                    var returnUrl = HttpContext.Session.GetString("ReturnUrl") ?? "/Commandes/Index";
                    HttpContext.Session.Remove("ReturnUrl");

                    return Redirect(returnUrl);
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
