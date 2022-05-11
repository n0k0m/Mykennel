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
using Mykennel.Models.ViewModels;
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
        public async Task<IActionResult> Index(string breed, string country, string hobby, int? pageNumber)
        {
            // Lenyíló listához az adatokat átadom a nézetnek, illetve ha már volt kiválasztva adat, akkor azt is visszaadom
            ViewData["BreedId"] = new SelectList(_context.Breeds, "BreedId", "Name");
            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryName");

            if (!String.IsNullOrEmpty(breed)) ViewData["BreedFilter"] = breed;
            if (!String.IsNullOrEmpty(country)) ViewData["CountryFilter"] = country;
            if (!String.IsNullOrEmpty(hobby)) ViewData["HobbyFilter"] = hobby;

            int pageSize = 5;
            if (String.IsNullOrEmpty(hobby))
            {
                var littersVM = (from l in _context.Litters
                                join k in _context.Kennels on l.KennelId equals k.KennelId
                                join u in _context.ApplicationUsers on k.ApplicationUserId equals u.Id
                                join p in _context.Puppies on l.LitterId equals p.LitterId
                                where p.Bookable && (u.LockoutEnd < DateTime.Now || u.LockoutEnd == null)
                                 select new LitterVM { Litter = l, Kennel = k, User = u, Breed = l.Mother.Breed, Country = l.Kennel.Country});

                if (!String.IsNullOrEmpty(breed))
                {
                    littersVM = (from l in littersVM
                                 where l.Breed.BreedId.Equals(int.Parse(breed))
                                 select l);
                }

                if (!String.IsNullOrEmpty(country))
                {
                    littersVM = (from l in littersVM
                                 where l.Country.CountryId.Equals(int.Parse(country))
                                 select l);
                }

                return View(await PaginatedList<LitterVM>.CreateAsync(littersVM.Distinct().OrderByDescending(l => l.Litter.Date), pageNumber ?? 1, pageSize));
            } else
            {
                var hobbyLittersVM = (from l in _context.Litters
                                      join k in _context.Kennels on l.KennelId equals k.KennelId
                                      join p in _context.Puppies on l.LitterId equals p.LitterId
                                      where p.Bookable && p.Aim == Aim.Hobby
                                      select new LitterVM { Litter = l, Kennel = k, Breed = l.Mother.Breed, Country = l.Kennel.Country });

                if (!String.IsNullOrEmpty(breed))
                {
                    hobbyLittersVM = (from l in hobbyLittersVM
                                      where l.Breed.BreedId.Equals(int.Parse(breed))
                                      select l);
                }

                if (!String.IsNullOrEmpty(country))
                {
                    hobbyLittersVM = (from l in hobbyLittersVM
                                 where l.Country.CountryId.Equals(int.Parse(country))
                                 select l);
                }

                return View(await PaginatedList<LitterVM>.CreateAsync(hobbyLittersVM.Distinct().OrderByDescending(l => l.Litter.Date), pageNumber ?? 1, pageSize));
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
                .Include(l => l.Kennel.ApplicationUser)
                .Include(l => l.Mother)
                .Include(l => l.Puppies)
                .Where(l => l.Kennel.ApplicationUser.LockoutEnd < DateTime.Now || l.Kennel.ApplicationUser.LockoutEnd == null)
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
            var userKennel = GetUserKennel();

            if (userKennel == null)
            {
                TempData["ErrorMessage"] = "You need to create a kennel first!";
                return RedirectToAction("Settings", "Kennels");
            }

            if (_context.Dogs.Where(m => m.KennelId == userKennel.KennelId).Count() < 2)
            {
                TempData["ErrorMessage"] = "You need to have at least 2 dogs to create a litter and add puppies!";
                return RedirectToAction("MyDogs", "Dogs");
            }

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
                try
                {
                    _context.Add(litter);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(MyLitters));
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "You need to add dogs before you can add a litter!";
                    return RedirectToAction(nameof(Create));
                }
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
