using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PatPilot.Data;
using PatPilot.Models;

namespace PatPilot.Controllers
{
    public class EnseignesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EnseignesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Enseignes
        public async Task<IActionResult> Index()
        {
              return _context.Enseigne != null ? 
                          View(await _context.Enseigne.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Enseigne'  is null.");
        }

        // GET: Enseignes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Enseigne == null)
            {
                return NotFound();
            }

            var enseigne = await _context.Enseigne
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enseigne == null)
            {
                return NotFound();
            }

            return View(enseigne);
        }

        // GET: Enseignes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Enseignes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Enseigne enseigne)
        {
            ModelState.Remove("Url");
            ModelState.Remove("Users");
            ModelState.Remove("Gateaux");

            if (ModelState.IsValid)
            {
                enseigne.Id = Guid.NewGuid();

                var request = HttpContext.Request;
                var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";

                enseigne.Url = $"{baseUrl}/Commandes/{enseigne.Id}";

                _context.Enseigne?.Add(enseigne);
                await _context.SaveChangesAsync();

                //Create User
                var user = new ApplicationUser
                {
                    UserName = enseigne.ContactName,
                    Email = enseigne.Email,
                    EnseigneId = enseigne.Id
                };

                var result = await _userManager.CreateAsync(user, enseigne.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "AdminEnseigne");
                    // Create enseigne directory
                    Utils.utils.CreateImageDirectory(enseigne.Name);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(enseigne);
        }

        // GET: Enseignes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Enseigne == null)
            {
                return NotFound();
            }

            var enseigne = await _context.Enseigne.FindAsync(id);
            if (enseigne == null)
            {
                return NotFound();
            }
            return View(enseigne);
        }

        // POST: Enseignes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Enseigne enseigne)
        {
            if (id != enseigne.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Url");
            ModelState.Remove("Users");
            ModelState.Remove("Gateaux");

            if (ModelState.IsValid)
            {
                try
                {
                    var request = HttpContext.Request;
                    var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";

                    enseigne.Url = $"{baseUrl}/Commandes/{enseigne.Id}";

                    _context.Update(enseigne);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnseigneExists(enseigne.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(enseigne);
        }

        // GET: Enseignes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Enseigne == null)
            {
                return NotFound();
            }

            var enseigne = await _context.Enseigne
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enseigne == null)
            {
                return NotFound();
            }

            return View(enseigne);
        }

        // POST: Enseignes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Enseigne == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Enseigne'  is null.");
            }
            var enseigne = await _context.Enseigne.FindAsync(id);
            if (enseigne != null)
            {
                _context.Enseigne.Remove(enseigne);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnseigneExists(Guid id)
        {
          return (_context.Enseigne?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
