using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mykennel.Data;
using Mykennel.Models;
using Mykennel.Utility;

namespace Mykennel.Controllers
{
    public class LittersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LittersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Kereső oldal
        // GET: Litters
        public async Task<IActionResult> Index(string breed, string country, string hobby)
        {
            List<Litter> litters = new List<Litter>();
            if (!String.IsNullOrEmpty(hobby))
            {
                litters = (litters = (from l in _context.Litters
                           join p in _context.Puppies on l.LitterId equals p.LitterId
                           where p.Bookable && p.Aim == Aim.Hobby
                           select l).Include(l => l.Kennel).Distinct().ToList());
            } else
            {
                litters = (from l in _context.Litters
                           join p in _context.Puppies on l.LitterId equals p.LitterId
                           where p.Bookable 
                           select l).Include(l => l.Kennel).Distinct().ToList();
            }

            foreach (Litter litter in litters)
            {
                litter.Breed = (from l in litters
                                join d in _context.Dogs on l.MotherId equals d.DogId
                                join b in _context.Breeds on d.BreedId equals b.BreedId
                                where l.LitterId.Equals(litter.LitterId)
                                select b).FirstOrDefault();
                litter.Country = (from l in litters
                                  join k in _context.Kennels on l.KennelId equals k.KennelId
                                  join c in _context.Countries on k.CountryId equals c.CountryId
                                  where l.LitterId.Equals(litter.LitterId)
                                  select c).FirstOrDefault();
            }

            ViewData["BreedId"] = new SelectList(_context.Breeds, "BreedId", "Name");
            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryName");

            if (!String.IsNullOrEmpty(breed) && !String.IsNullOrEmpty(country))
            {
                return View(litters.Where(m => m.Breed.BreedId == int.Parse(breed) && m.Country.CountryId == int.Parse(country)).ToList());
            }
            else if (!String.IsNullOrEmpty(breed))
            {
                return View(litters.Where(m => m.Breed.BreedId == int.Parse(breed)).ToList());
            }
            else if (!String.IsNullOrEmpty(country))
            {
                return View(litters.Where(m => m.Country.CountryId == int.Parse(country)).ToList());
            }
            else
            {
                return View(litters.ToList());
            }
        }

        // Az alom saját oldala
        // GET: Litters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var litter = await _context.Litters
                .Include(l => l.Father)
                .Include(l => l.Kennel)
                .Include(l => l.Mother)
                .Include(l => l.Puppies)
                .FirstOrDefaultAsync(m => m.LitterId == id);
            if (litter == null)
            {
                return NotFound();
            }

            ViewBag.Breed = (from l in _context.Litters
                             join d in _context.Dogs on l.MotherId equals d.DogId
                            join b in _context.Breeds on d.BreedId equals b.BreedId
                            where l.LitterId.Equals(litter.LitterId)
                            select b).FirstOrDefault();
            return View(litter);
        }

        // GET: Litters/MyLitters
        public IActionResult MyLitters()
        {
            var userLitters = GetUserLitters();

            return View(userLitters);
        }

        // GET: Litters/Create
        [Authorize(Roles = SD.Role_User_Breeder)]
        public IActionResult Create()
        {
            int userKennelId = GetUserKennel().KennelId;
            ViewData["FatherId"] = new SelectList(_context.Dogs.Where(m => m.KennelId == userKennelId && (int)m.Sex == 1), "DogId", "Name");
            ViewData["KennelId"] = new SelectList(_context.Kennels.Where(m => m.KennelId == userKennelId), "KennelId", "KennelName");
            ViewData["MotherId"] = new SelectList(_context.Dogs.Where(m => m.KennelId == userKennelId && (int)m.Sex == 2), "DogId", "Name");
            return View();
        }

        // POST: Litters/Create
        [Authorize(Roles = SD.Role_User_Breeder)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LitterId,Name,Date,KennelId,FatherId,MotherId")] Litter litter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(litter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyLitters));
            }
            int userKennelId = GetUserKennel().KennelId;
            ViewData["FatherId"] = new SelectList(_context.Dogs.Where(m => m.KennelId == userKennelId), "DogId", "Name", litter.FatherId);
            ViewData["KennelId"] = new SelectList(_context.Kennels.Where(m => m.KennelId == userKennelId), "KennelId", "Address", litter.KennelId);
            ViewData["MotherId"] = new SelectList(_context.Dogs.Where(m => m.KennelId == userKennelId), "DogId", "Name", litter.MotherId);
            return View(litter);
        }

        // GET: Litters/Edit/5
        [Authorize(Roles = SD.Role_User_Breeder)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var litter = await _context.Litters.FindAsync(id);
            if (litter == null || litter.KennelId != GetUserKennel().KennelId)
            {
                return NotFound();
            }
            int userKennelId = GetUserKennel().KennelId;
            ViewData["FatherId"] = new SelectList(_context.Dogs.Where(m => m.KennelId == userKennelId), "DogId", "Name", litter.FatherId);
            ViewData["KennelId"] = new SelectList(_context.Kennels.Where(m => m.KennelId == userKennelId), "KennelId", "Address", litter.KennelId);
            ViewData["MotherId"] = new SelectList(_context.Dogs.Where(m => m.KennelId == userKennelId), "DogId", "Name", litter.MotherId);
            return View(litter);
        }

        // POST: Litters/Edit/5
        [Authorize(Roles = SD.Role_User_Breeder)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LitterId,Name,Date,KennelId,FatherId,MotherId")] Litter litter)
        {
            if (id != litter.LitterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(litter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LitterExists(litter.LitterId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MyLitters));
            }
            int userKennelId = GetUserKennel().KennelId;
            ViewData["FatherId"] = new SelectList(_context.Dogs.Where(m => m.KennelId == userKennelId), "DogId", "Name", litter.FatherId);
            ViewData["KennelId"] = new SelectList(_context.Kennels.Where(m => m.KennelId == userKennelId), "KennelId", "Address", litter.KennelId);
            ViewData["MotherId"] = new SelectList(_context.Dogs.Where(m => m.KennelId == userKennelId), "DogId", "Name", litter.MotherId);
            return View(litter);
        }

        // GET: Litters/Delete/5
        [Authorize(Roles = SD.Role_User_Breeder)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var litter = await _context.Litters
                .Include(l => l.Father)
                .Include(l => l.Kennel)
                .Include(l => l.Mother)
                .FirstOrDefaultAsync(m => m.LitterId == id);
            if (litter == null || litter.KennelId != GetUserKennel().KennelId)
            {
                return NotFound();
            }

            return View(litter);
        }

        // POST: Litters/Delete/5
        [Authorize(Roles = SD.Role_User_Breeder)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var litter = await _context.Litters.FindAsync(id);
            _context.Litters.Remove(litter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyLitters));
        }

        private bool LitterExists(int id)
        {
            return _context.Litters.Any(e => e.LitterId == id);
        }

        // Saját metódusaim
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private Kennel GetUserKennel()
        {
            string id = GetUserId();
            var userKennel = _context.Kennels.FirstOrDefault(m => m.ApplicationUserId == id);
            return userKennel;
        }

        private IQueryable<Litter> GetUserLitters()
        {
            int id = GetUserKennel().KennelId;

            var userLitters = _context.Litters
                .Include(l => l.Father)
                .Include(l => l.Kennel)
                .Include(l => l.Mother)
                .Where(m => m.KennelId == id);

            return userLitters;
        }
    }
}
