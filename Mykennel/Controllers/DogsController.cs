using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mykennel.Data;
using Mykennel.Models;
using Mykennel.Utility;

namespace Mykennel.Controllers
{
    public class DogsController : Controller
    {
        private readonly ApplicationDbContext _context;
        // Szükséges a képek mappa eléréséhez
        private readonly IWebHostEnvironment _hostEnvironment;

        public DogsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Dogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dog = await _context.Dogs
                .Include(d => d.Breed)
                .Include(d => d.Father)
                .Include(d => d.Kennel)
                .Include(d => d.Mother)
                .FirstOrDefaultAsync(m => m.DogId == id);
            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // GET: Dogs/MyDogs
        public IActionResult MyDogs()
        {
            var userDogs = GetUserDogs();

            return View(userDogs);
        }

        // GET: Dogs/Create
        [Authorize(Roles = SD.Role_User_Breeder)]
        public IActionResult Create()
        {
            ViewData["BreedId"] = new SelectList(_context.Breeds, "BreedId", "Name");
            ViewData["FatherId"] = new SelectList(_context.Dogs.Where(m => m.KennelId == GetUserKennel().KennelId), "DogId", "Name");
            ViewData["KennelId"] = new SelectList(_context.Kennels.Where(m => m.KennelId == GetUserKennel().KennelId), "KennelId", "KennelName");
            ViewData["MotherId"] = new SelectList(_context.Dogs.Where(m => m.KennelId == GetUserKennel().KennelId), "DogId", "Name");
            return View();
        }

        // POST: Dogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = SD.Role_User_Breeder)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DogId,RegNumber,Born,Name,Sex,Description,TitlesGenetics,Status,DogImage,BreedId,KennelId,FatherId,MotherId")] Dog dog)
        {
            if (ModelState.IsValid)
            {         
                
                string webRootPath = _hostEnvironment.WebRootPath;
                var file = HttpContext.Request.Form.Files;
                // Ha lett kép kiválasztva, akkor csinálni kell valamit
                if (file.Count > 0)
                {
                    // útvonal, fájlnév, kiterjesztés összerakása
                    string fileName = dog.DogId.ToString() + "_" + Guid.NewGuid().ToString();
                    string kennelFolder = Path.Combine("kennels", dog.KennelId.ToString(), "dogs");
                    string uploadFolder = Path.Combine(webRootPath, kennelFolder);
                    string extension = Path.GetExtension(file[0].FileName);

                    // Mappa létrehozása ha nem létezik
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    // Kép mentése FileStream-el
                    using (var fStream = new FileStream(Path.Combine(uploadFolder, fileName + extension), FileMode.Create))
                    {
                        file[0].CopyTo(fStream);
                    }
                    dog.DogImage = @"\" + Path.Combine(kennelFolder, fileName + extension);
                }

                _context.Add(dog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyDogs));
            }
            ViewData["BreedId"] = new SelectList(_context.Breeds, "BreedId", "Name", dog.BreedId);
            ViewData["FatherId"] = new SelectList(_context.Dogs.Where(m => m.KennelId == GetUserKennel().KennelId), "DogId", "Name", dog.FatherId);
            ViewData["KennelId"] = new SelectList(_context.Kennels.Where(m => m.KennelId == GetUserKennel().KennelId), "KennelId", "KennelName");
            ViewData["MotherId"] = new SelectList(_context.Dogs.Where(m => m.KennelId == GetUserKennel().KennelId), "DogId", "Name", dog.MotherId);
            return View(dog);
        }

        // GET: Dogs/Edit/5
        [Authorize(Roles = SD.Role_User_Breeder)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dog = await _context.Dogs.FindAsync(id);
            if (dog == null || dog.KennelId != GetUserKennel().KennelId)
            {
                return NotFound();
            }

            ViewData["BreedId"] = new SelectList(_context.Breeds, "BreedId", "Name", dog.BreedId);
            ViewData["FatherId"] = new SelectList(_context.Dogs.Where(m => m.KennelId == GetUserKennel().KennelId), "DogId", "Name", dog.FatherId);
            ViewData["KennelId"] = new SelectList(_context.Kennels.Where(m => m.KennelId == GetUserKennel().KennelId), "KennelId", "KennelName");
            ViewData["MotherId"] = new SelectList(_context.Dogs.Where(m => m.KennelId == GetUserKennel().KennelId), "DogId", "Name", dog.MotherId);
            return View(dog);
        }

        // POST: Dogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = SD.Role_User_Breeder)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DogId,RegNumber,Born,Name,Sex,Description,TitlesGenetics,Status,DogImage,BreedId,KennelId,FatherId,MotherId")] Dog dog)
        {
            if (id != dog.DogId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string webRootPath = _hostEnvironment.WebRootPath;
                    var file = HttpContext.Request.Form.Files;
                    // Ha lett kép kiválasztva, akkor csinálni kell valamit
                    if (file.Count > 0)
                    {
                        string fileName = dog.DogId.ToString() + "_" + Guid.NewGuid().ToString();
                        string kennelFolder = Path.Combine("kennels", dog.KennelId.ToString(), "dogs");
                        string uploadFolder = Path.Combine(webRootPath, kennelFolder); //összerakjuk az útvonalat hogy hova mentse
                        string extension = Path.GetExtension(file[0].FileName);

                        // Ha volt már kép, akkor ki kell törölni a régit
                        if (dog.DogImage != null)
                        {
                            var imagePath = Path.Combine(webRootPath, dog.DogImage.TrimStart('\\'));
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }
                        }

                        if (!Directory.Exists(uploadFolder))
                        {
                            Directory.CreateDirectory(uploadFolder);
                        }

                        using (var fStream = new FileStream(Path.Combine(uploadFolder, fileName + extension), FileMode.Create))
                        {
                            file[0].CopyTo(fStream);
                        }
                        dog.DogImage = @"\" + Path.Combine(kennelFolder, fileName + extension);
                    }

                    _context.Update(dog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DogExists(dog.DogId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MyDogs));
            }
            ViewData["BreedId"] = new SelectList(_context.Breeds, "BreedId", "Name", dog.BreedId);
            ViewData["FatherId"] = new SelectList(_context.Dogs.Where(m => m.KennelId == GetUserKennel().KennelId), "DogId", "Name", dog.FatherId);
            ViewData["KennelId"] = new SelectList(_context.Kennels.Where(m => m.KennelId == GetUserKennel().KennelId), "KennelId", "KennelName");
            ViewData["MotherId"] = new SelectList(_context.Dogs.Where(m => m.KennelId == GetUserKennel().KennelId), "DogId", "Name", dog.MotherId);
            return View(dog);
        }

        // GET: Dogs/Delete/5
        [Authorize(Roles = SD.Role_User_Breeder)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dog = await _context.Dogs
                .Include(d => d.Breed)
                .Include(d => d.Father)
                .Include(d => d.Kennel)
                .Include(d => d.Mother)
                .FirstOrDefaultAsync(m => m.DogId == id);
            if (dog == null || dog.KennelId != GetUserKennel().KennelId)
            {
                return NotFound();
            }

            return View(dog);
        }

        // POST: Dogs/Delete/5
        [Authorize(Roles = SD.Role_User_Breeder)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dog = await _context.Dogs.FindAsync(id);
            _context.Dogs.Remove(dog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyDogs));
        }

        private bool DogExists(int id)
        {
            return _context.Dogs.Any(e => e.DogId == id);
        }

        // Saját metódusaim:
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

        private IQueryable<Dog> GetUserDogs()
        {

            int id = GetUserKennel().KennelId;

            var userDogs = _context.Dogs
                .Include(d => d.Breed)
                .Include(d => d.Father)
                .Include(d => d.Kennel)
                .Include(d => d.Mother)
                .Where(m => m.KennelId == id);

            return userDogs;
        }
    }
}
