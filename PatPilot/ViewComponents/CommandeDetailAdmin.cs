using Microsoft.AspNetCore.Mvc;
using PatPilot.Models;

namespace PatPilot.ViewComponents
{
    public class CommandeDetailAdminViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Commande commande)
        {
            return View("AdminTemplate", commande); // nom du template spécifique pour Admin
        }
    }
}
