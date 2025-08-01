using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PatPilot.Data;
using PatPilot.Models;
using PatPilot.Models.DTO;
using PatPilot.Utils;
using PatPilot.ViewModels;
using System.Text.Json;

namespace PatPilot.Controllers
{
    public class ClientAccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ClientAccountController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManage)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManage;
        }

        [HttpGet]
        public IActionResult RegisterClient()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterClient(RegisterClientViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            //on récupère l'enseigne de la session
            var enseigneJson = HttpContext.Session.GetString(Constantes.S_enseigne_client);
            EnseigneDTO enseigne = null;
            if (!string.IsNullOrEmpty(enseigneJson))
            {
                enseigne = JsonSerializer.Deserialize<EnseigneDTO>(enseigneJson);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                Adresse = model.Adresse,
                EnseigneId = enseigne?.Id
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");

                // Créer le client lié à l'utilisateur
                var client = CopyFromUser(user);
                client.Prenom = model.LastName;
                client.Telephone = model.Tel;
                client.Adresse = model.Adresse;

                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                // Authentifier
                await _signInManager.SignInAsync(user, isPersistent: false);
                HttpContext.Session.SetString(Constantes.S_client_id_client, client.Id.ToString());
                HttpContext.Session.SetString(Constantes.S_client_client, JsonSerializer.Serialize(client));

                var returnUrl = HttpContext.Session.GetString("ReturnUrl") ?? "/Commandes/Index";
                HttpContext.Session.Remove("ReturnUrl");

                return Redirect(returnUrl);
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        private Client CopyFromUser(ApplicationUser user)
        {
            var client = new Client();
            client.UserId = user.Id;
            if (user.EnseigneId.HasValue)
                client.EnseigneId = user.EnseigneId.Value;
            client.Nom = user.UserName; 
            client.Email = user.Email;
            client.MotDePasse = user.PasswordHash;

            return client;
        }
    }
}
