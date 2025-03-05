using Microsoft.AspNetCore.Mvc;
using PatPilot.Models;
using PatPilot.Models.DTO;
using PatPilot.Repositories;
using System.Text.Json;

namespace PatPilot.Controllers
{
    public class GateauxController : Controller
    {
        private readonly IGateauRepository _gateauRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GateauxController(IGateauRepository gateauRepository, IHttpContextAccessor httpContextAccessor)
        {
            _gateauRepository = gateauRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            var ensid = Guid.Empty;
            var enseigneJson = _httpContextAccessor.HttpContext.Session.GetString("Enseigne");
            if(enseigneJson != null)
            {
                var enseigneDto = JsonSerializer.Deserialize <EnseigneDTO >(enseigneJson);
                if (enseigneDto != null) ensid = enseigneDto.Id;
                else return Unauthorized();
            }
            

            var gateaux = await _gateauRepository.GetAllByEnseigneAsync(ensid);
            return View(gateaux);
        }

        [HttpPost]
        public async Task<IActionResult> SaveGateau(Gateau gateau, IFormFile ImageFile)
        {
            ModelState.Remove("Ingredients");
            ModelState.Remove("Enseigne");
            ModelState.Remove("Id");
            ModelState.Remove("Image");

            if (ModelState.IsValid)
            {
                //Save Image
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string enseigneName = Utils.utils.FormatDirectoryName(_httpContextAccessor.HttpContext.Session.GetString("EnseigneName"));
                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", enseigneName);

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    string filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    // Sauvegarder uniquement le chemin relatif
                    gateau.Image = Path.Combine("img", enseigneName, fileName);
                }

                if (gateau.Id == Guid.Empty)
                {
                    gateau.Id = Guid.NewGuid();
                    await _gateauRepository.AddAsync(gateau);
                }
                else
                {
                    //
                    await _gateauRepository.UpdateAsync(gateau);
                }
                return RedirectToAction("Index");
            }
            return View("Index");
        }
    }
}
