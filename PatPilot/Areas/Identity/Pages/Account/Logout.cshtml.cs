using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using PatPilot.Models;
using PatPilot.Utils;

namespace PatPilot.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(User);
            string roleRedirect = "/Home/Index"; // par défaut pour Admin/AdminEnseigne

            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Customer"))
                {
                    roleRedirect = "/Commandes/Index";

                    // Vider la session
                    HttpContext.Session.Remove(Constantes.S_client_client);
                    HttpContext.Session.Remove(Constantes.S_client_id_client);
                    //HttpContext.Session.Remove(Constantes.S_enseigne_client);
                }
                else
                {
                    //vider la session Admin
                }
            }

            await _signInManager.SignOutAsync();

            

            return LocalRedirect(roleRedirect);
        }
    }
}
