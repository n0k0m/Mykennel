using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mykennel.Data;
using Mykennel.Models;
using Mykennel.Utility;

namespace Mykennel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class BreedsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BreedsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Breeds
        public async Task<IActionResult> Index()
        {
            return View(await _context.Breeds.ToListAsync());
        }

        // GET: Admin/Breeds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var breed = await _context.Breeds
                .FirstOrDefaultAsync(m => m.BreedId == id);
            if (breed == null)
            {
                return NotFound();
            }

            return View(breed);
        }

        // GET: Admin/Breeds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Breeds/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BreedId,Name,OriginalName")] Breed breed)
        {
            if ((from b in _context.Breeds where b.BreedId == breed.BreedId select b).FirstOrDefault() != null)
            {
                TempData["BreedError"] = "FCI number already registered!";
                return View(breed);
            }

            if (ModelState.IsValid)
            {
                _context.Add(breed);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(breed);
        }

        // GET: Admin/Breeds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var breed = await _context.Breeds.FindAsync(id);
            if (breed == null)
            {
                return NotFound();
            }
            return View(breed);
        }

        // POST: Admin/Breeds/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BreedId,Name,OriginalName")] Breed breed)
        {
            if (id != breed.BreedId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(breed);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BreedExists(breed.BreedId))
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
            return View(breed);
        }

        // GET: Admin/Breeds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // Nem akarom, hogy törölni lehessen, ha már van regisztrálva kutya ehhez a fajtához.
            var dogsInBreed = (from d in _context.Dogs
                               join k in _context.Kennels on d.KennelId equals k.KennelId
                                    where d.BreedId == id
                                    select new { k.KennelName, d.Name }).ToList();

            if (dogsInBreed.Any())
            {
                TempData["BreedError2"] = "Can't delete breed with dogs registered to it: " + String.Join(", ", dogsInBreed);
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
            {
                return NotFound();
            }

            var breed = await _context.Breeds
                .FirstOrDefaultAsync(m => m.BreedId == id);
            if (breed == null)
            {
                return NotFound();
            }

            return View(breed);
        }

        // POST: Admin/Breeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var breed = await _context.Breeds.FindAsync(id);
            _context.Breeds.Remove(breed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BreedExists(int id)
        {
            return _context.Breeds.Any(e => e.BreedId == id);
        }
    }
}
