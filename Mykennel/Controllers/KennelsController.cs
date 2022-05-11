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
    public class KennelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        // Szükséges a képek mappa eléréséhez
        private readonly IWebHostEnvironment _hostEnvironment;

        public KennelsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // Ez lesz a kennelek kereshető oldala
        // GET: Kennels
        public async Task<IActionResult> Index(string breed, string country, int? pageNumber)
        {  
            var kennels = _context.Kennels;

            // Feltölöm a fajtalistát az adott kennelhez
            await foreach (Kennel kennel in kennels)
            {
                var breeds = (from d in _context.Dogs
                 where d.KennelId.Equals(kennel.KennelId)
                 select d.Breed).Distinct();
                kennel.Breeds = breeds;
            }

            // Lenyíló listához az adatokat átadom a nézetnek, illetve ha már volt kiválasztva adat, akkor azt is visszaadom
            ViewData["BreedId"] = new SelectList(_context.Breeds, "BreedId", "Name");
            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryName");

            if (!String.IsNullOrEmpty(breed))
            {
                ViewData["BreedFilter"] = breed;
            }
            
            if (!String.IsNullOrEmpty(country))
            {
                ViewData["CountryFilter"] = country;
            }

            // Hány elem jelenjen meg egy oldalon
            int pageSize = 5;

            // Szűrők
            if (!String.IsNullOrEmpty(breed) && !String.IsNullOrEmpty(country))
            {
                return View(await PaginatedList<Kennel>.CreateAsync(
                            ((from k in kennels
                            join u in _context.ApplicationUsers on k.ApplicationUserId equals u.Id
                            join c in _context.Countries on k.CountryId equals c.CountryId
                            join d in _context.Dogs on k.KennelId equals d.KennelId
                            join b in _context.Breeds on d.BreedId equals b.BreedId
                            where b.BreedId.Equals(int.Parse(breed)) && c.CountryId.Equals(int.Parse(country)) && (u.LockoutEnd < DateTime.Now || u.LockoutEnd == null)
                            orderby k.KennelName
                            select k).Distinct())
                    , pageNumber ?? 1, pageSize));
            }
            else if (!String.IsNullOrEmpty(breed))
            {
                return View(await PaginatedList<Kennel>.CreateAsync(
                   ((from k in kennels
                           join u in _context.ApplicationUsers on k.ApplicationUserId equals u.Id
                           join c in _context.Countries on k.CountryId equals c.CountryId
                           join d in _context.Dogs on k.KennelId equals d.KennelId
                           join b in _context.Breeds on d.BreedId equals b.BreedId
                           where b.BreedId.Equals(int.Parse(breed)) && (u.LockoutEnd < DateTime.Now || u.LockoutEnd == null)
                           orderby k.KennelName
                           select k).Distinct())
                    , pageNumber ?? 1, pageSize));
            }
            else if (!String.IsNullOrEmpty(country))
            {
                return View(await PaginatedList<Kennel>.CreateAsync(
                    ((from k in kennels
                            join u in _context.ApplicationUsers on k.ApplicationUserId equals u.Id
                            join c in _context.Countries on k.CountryId equals c.CountryId
                            join d in _context.Dogs on k.KennelId equals d.KennelId
                            join b in _context.Breeds on d.BreedId equals b.BreedId
                            where c.CountryId.Equals(int.Parse(country)) && (u.LockoutEnd < DateTime.Now || u.LockoutEnd == null)
                            orderby k.KennelName
                            select k).Distinct())
                    , pageNumber ?? 1, pageSize));
            }
            else
            {
                return View(await PaginatedList<Kennel>.CreateAsync(
                    ((from k in kennels
                            join u in _context.ApplicationUsers on k.ApplicationUserId equals u.Id
                            join c in _context.Countries on k.CountryId equals c.CountryId
                            join d in _context.Dogs on k.KennelId equals d.KennelId
                            join b in _context.Breeds on d.BreedId equals b.BreedId
                            where u.LockoutEnd < DateTime.Now || u.LockoutEnd == null
                            orderby k.KennelName
                            select k).Distinct())
                    , pageNumber ?? 1, pageSize));
            }
        }

        // Ez lesz a kennel saját oldala
        // GET: Kennels/
        [HttpGet("Kennels/Details/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            if (slug == null)
            {
                return NotFound();
            }

            var kennel = await _context.Kennels
                .Include(k => k.ApplicationUser)
                .Include(k => k.Country)
                .Include(k => k.Dogs)
                .Include(k => k.Litters)
                .Where(k => k.ApplicationUser.LockoutEnd < DateTime.Now || k.ApplicationUser.LockoutEnd == null)
                .FirstOrDefaultAsync(m => m.URLName == slug.ToLower());

            if (kennel == null)
            {
                return NotFound();
            }

            ViewBag.KennelBreeds = GetKennelBreeds(kennel.KennelId);
            return View(kennel);
        }

        // Extra védelmi réteg, hogy véletlen se tudja "normál felhasználó" elérni az action-t
        // GET: Settings
        [Authorize(Roles = SD.Role_User_Breeder)]
        public IActionResult Settings()
        {
            var userKennel = GetUserKennel();

            // Ha van már kennel létrehozva
            if (GetUserKennel() != null)
            {
                ViewData["AlreadyExists"] = true;

                // Adjuk vissza az adatbázisban lévő adatokat
                return View(userKennel);
            }

            return View();
        }

        // GET: Kennels/Create
        [Authorize(Roles = SD.Role_User_Breeder)]
        public IActionResult Create()
        {
            // Egyelőre csak egyetlen kennelt szeretnék hogy egy felhasználó létre tudjon hozni
            if(GetUserKennel() == null)
            {
                ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryName");
                return View();
            }
            else
            {
                return RedirectToAction(nameof(Settings));
            }
            
        }

        // POST: Kennels/Create
        [Authorize(Roles = SD.Role_User_Breeder)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KennelId,KennelName,URLName,City,PostalCode,Address,ShortDescription,Description,Logo,ContactPerson,CountryId")] Kennel kennel)
        {
            if ((from k in _context.Kennels where k.URLName == kennel.URLName select k).FirstOrDefault() != null)
            {
                TempData["URLError"] = "URL name is taken, please choose another one!";
                ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryName", kennel.CountryId);
                return View(kennel);
            }

            if (ModelState.IsValid)
            {
                // Automatikusan az éppen aktuális felhasználóhoz kötjük
                kennel.ApplicationUserId = GetUserId();

                _context.Add(kennel);
                await _context.SaveChangesAsync();

                string webRootPath = _hostEnvironment.WebRootPath;
                var file = HttpContext.Request.Form.Files;

                // Ha lett kép kiválasztva: újra le kell kérdezni, hogy meglegyen a kennelId
                // újra le kell menteni az elérési utat
                if (file.Count > 0)
                {
                    kennel = (from k in _context.Kennels where k.ApplicationUserId == GetUserId() select k).FirstOrDefault();

                    string fileName = "logo" + "_" + Guid.NewGuid().ToString();
                    string kennelFolder = Path.Combine("kennels", kennel.KennelId.ToString());
                    string uploadFolder = Path.Combine(webRootPath, kennelFolder); //összerakjuk az útvonalat hogy hova mentse
                    string extension = Path.GetExtension(file[0].FileName);

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    using (var fStream = new FileStream(Path.Combine(uploadFolder, fileName + extension), FileMode.Create))
                    {
                        file[0].CopyTo(fStream);
                    }
                    kennel.Logo = @"\" + Path.Combine(kennelFolder, fileName + extension);
                    _context.Update(kennel);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Settings));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryName", kennel.CountryId);
            return View(kennel);
        }

        // GET: Kennels/Edit
        [Authorize(Roles = SD.Role_User_Breeder)]
        public async Task<IActionResult> Edit()
        {
            int? id = GetUserKennel().KennelId;

            if (id == null)
            {
                return NotFound();
            }

            var kennel = await _context.Kennels.FindAsync(id);
            if (kennel == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryName", kennel.CountryId);
            return View(kennel);
        }

        // POST: Kennels/Edit
        [Authorize(Roles = SD.Role_User_Breeder)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("KennelId,KennelName,URLName,City,PostalCode,Address,ShortDescription,Description,Logo,ContactPerson,CountryId,ApplicationUserId")] Kennel kennel)
        {
            int? id = kennel.KennelId;

            if (id == null)
            {
                return NotFound();
            }

            // Megvizsgálom, hogy az URL nevet regisztrálta e már valaki
            if ((from k in _context.Kennels where k.URLName == kennel.URLName && k.KennelId != kennel.KennelId select k).FirstOrDefault() != null)
            {
                TempData["URLError"] = "URL name is taken, please choose another one!";
                ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryName", kennel.CountryId);
                return View(kennel);
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
                        string fileName = "logo" + "_" + Guid.NewGuid().ToString();
                        string kennelFolder = Path.Combine("kennels", kennel.KennelId.ToString());
                        string uploadFolder = Path.Combine(webRootPath, kennelFolder); //összerakjuk az útvonalat hogy hova mentse
                        string extension = Path.GetExtension(file[0].FileName);

                        // Ha volt már kép, akkor ki kell törölni a régit
                        if (kennel.Logo != null)
                        {
                            var imagePath = Path.Combine(webRootPath, kennel.Logo.TrimStart('\\'));
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }
                        }

                        // Ha nem nyitott még könyvtárat a kennelnek, akkor csinálni kell egyet
                        if (!Directory.Exists(uploadFolder))
                        {
                            Directory.CreateDirectory(uploadFolder);
                        }

                        using (var fStream = new FileStream(Path.Combine(uploadFolder, fileName + extension), FileMode.Create))
                        {
                            file[0].CopyTo(fStream);
                        }
                        kennel.Logo = @"\" + Path.Combine(kennelFolder, fileName + extension);
                    }

                    _context.Update(kennel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KennelExists(kennel.KennelId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Settings));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryName", kennel.CountryId);
            return View(kennel);
        }

        // GET: Kennels/Delete
        [Authorize(Roles = SD.Role_User_Breeder)]
        public async Task<IActionResult> Delete()
        {
            int? id = GetUserKennel().KennelId;

            if (id == null)
            {
                return NotFound();
            }

            var kennel = await _context.Kennels
                .Include(k => k.ApplicationUser)
                .Include(k => k.Country)
                .FirstOrDefaultAsync(m => m.KennelId == id);
            if (kennel == null)
            {
                return NotFound();
            }

            return View(kennel);
        }

        // POST: Kennels/Delete
        [Authorize(Roles = SD.Role_User_Breeder)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed()
        {
            int? id = GetUserKennel().KennelId;

            var kennel = await _context.Kennels.FindAsync(id);

            string webRootPath = _hostEnvironment.WebRootPath;
            string kennelFolder = Path.Combine("kennels", kennel.KennelId.ToString());
            string uploadFolder = Path.Combine(webRootPath, kennelFolder);

            try
            {
                _context.Kennels.Remove(kennel);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Please remove all references before removing your kennel!";
                return RedirectToAction(nameof(Delete), kennel);
            }

            // Ki kell törölni a fájlokat is
            try
            {
                if (Directory.Exists(uploadFolder))
                {
                    DirectoryInfo di = new DirectoryInfo(uploadFolder);
                    foreach (FileInfo file in di.EnumerateFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.EnumerateDirectories())
                    {
                        dir.Delete(true);
                    }
                    Directory.Delete(uploadFolder);
                }
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Settings));
            }

            return RedirectToAction(nameof(Settings));
        }

        private bool KennelExists(int id)
        {
            return _context.Kennels.Any(e => e.KennelId == id);
        }

        // Saját metódusaim:

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private Kennel GetUserKennel()
        {
            string id = GetUserId();
            var userKennel = _context.Kennels
                .Include(k => k.ApplicationUser)
                .Include(k => k.Country)
                .FirstOrDefault(m => m.ApplicationUserId == id);
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

        private IQueryable<Breed> GetKennelBreeds(int? id)
        {
            return (from d in _context.Dogs
                    where d.KennelId.Equals(id)
                    select d.Breed).Distinct();
        }

    }
}
