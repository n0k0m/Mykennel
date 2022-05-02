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
    public class PuppiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PuppiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Puppies/MyPuppies
        public IActionResult MyPuppies()
        {
            var userPuppies = GetUserPuppies();

            return View(userPuppies);
        }

        // GET: Puppies/Create
        [Authorize(Roles = SD.Role_User_Breeder)]
        public IActionResult Create()
        {
            int userKennelId = GetUserKennel().KennelId;
            ViewData["DogId"] = new SelectList(_context.Dogs.Where(m => m.KennelId == userKennelId), "DogId", "Name");
            ViewData["LitterId"] = new SelectList(_context.Litters.Where(m => m.KennelId == userKennelId), "LitterId", "Name");
            return View();
        }

        // POST: Puppies/Create
        [Authorize(Roles = SD.Role_User_Breeder)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PuppyId,Name,Sex,Bookable,Aim,Description,DogId,LitterId")] Puppy puppy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(puppy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyPuppies));
            }
            int userKennelId = GetUserKennel().KennelId;
            ViewData["DogId"] = new SelectList(_context.Dogs.Where(m => m.KennelId == userKennelId), "DogId", "Name", puppy.DogId);
            ViewData["LitterId"] = new SelectList(_context.Litters.Where(m => m.KennelId == userKennelId), "LitterId", "Name", puppy.LitterId);
            return View(puppy);
        }

        // GET: Puppies/Edit/5
        [Authorize(Roles = SD.Role_User_Breeder)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var puppy = await _context.Puppies
                .Include(p => p.Dog)
                .Include(p => p.Litter)
                .FirstOrDefaultAsync(m => m.PuppyId == id);
            if (puppy == null || puppy.Litter.KennelId != GetUserKennel().KennelId)
            {
                return NotFound();
            }
            int userKennelId = GetUserKennel().KennelId;
            ViewData["DogId"] = new SelectList(_context.Dogs.Where(m => m.KennelId == userKennelId), "DogId", "Name", puppy.DogId);
            ViewData["LitterId"] = new SelectList(_context.Litters.Where(m => m.KennelId == userKennelId), "LitterId", "Name", puppy.LitterId);
            return View(puppy);
        }

        // POST: Puppies/Edit/5
        [Authorize(Roles = SD.Role_User_Breeder)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PuppyId,Name,Sex,Bookable,Aim,Description,DogId,LitterId")] Puppy puppy)
        {
            if (id != puppy.PuppyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(puppy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PuppyExists(puppy.PuppyId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MyPuppies));
            }
            int userKennelId = GetUserKennel().KennelId;
            ViewData["DogId"] = new SelectList(_context.Dogs.Where(m => m.KennelId == userKennelId), "DogId", "Name", puppy.DogId);
            ViewData["LitterId"] = new SelectList(_context.Litters.Where(m => m.KennelId == userKennelId), "LitterId", "Name", puppy.LitterId);
            return View(puppy);
        }

        // GET: Puppies/Delete/5
        [Authorize(Roles = SD.Role_User_Breeder)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var puppy = await _context.Puppies
                .Include(p => p.Dog)
                .Include(p => p.Litter)
                .FirstOrDefaultAsync(m => m.PuppyId == id);
            if (puppy == null || puppy.Litter.KennelId != GetUserKennel().KennelId)
            {
                return NotFound();
            }

            return View(puppy);
        }

        // POST: Puppies/Delete/5
        [Authorize(Roles = SD.Role_User_Breeder)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var puppy = await _context.Puppies
                .Include(p => p.Dog)
                .Include(p => p.Litter)
                .FirstOrDefaultAsync(m => m.PuppyId == id);
            _context.Puppies.Remove(puppy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyPuppies));
        }

        private bool PuppyExists(int id)
        {
            return _context.Puppies.Any(e => e.PuppyId == id);
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
        private IQueryable<Puppy> GetUserPuppies()
        {
            int id = GetUserKennel().KennelId;

            var userPuppies = _context.Puppies
                .Include(p => p.Dog)
                .Include(p => p.Litter)
                .Where(m => m.Litter.KennelId == id);

            return userPuppies;
        }
    }
}
