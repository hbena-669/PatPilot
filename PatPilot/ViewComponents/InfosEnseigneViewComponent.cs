using Microsoft.AspNetCore.Mvc;
using PatPilot.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using PatPilot.Models;
using System.Text.Json;
using PatPilot.Repositories;

namespace PatPilot.ViewComponents
{
    public class InfosEnseigneViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly PanierRepository _panierRepository;

        public InfosEnseigneViewComponent(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _panierRepository = new PanierRepository(httpContextAccessor);
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            
            var enseigne =  _panierRepository.getEnseigneFromSession();
            if (enseigne != null)
            {
                return Task.FromResult<IViewComponentResult>(View("Default", enseigne));
            }

            return Task.FromResult<IViewComponentResult>(View("Default", new Enseigne()));
        }
    }
}
