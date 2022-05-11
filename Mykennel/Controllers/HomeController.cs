using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mykennel.Data;
using Mykennel.Models;
using Mykennel.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Mykennel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            if (_context.Users.FirstOrDefault(u => u.Email == "admin@admin.eu") != null)
            { ViewBag.FindAdmin = true; }
            else
            { ViewBag.FindAdmin = false; }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // 3 számjegyű hibakódok kezelésére
        [Route("/Home/Oops/{code:int}")]
        public IActionResult Oops(int code)
        {
            ViewData["ErrorCode"] = $"{code}";
            return View("~/Views/Shared/Oops.cshtml");
        }

        // Végleges kódban természetesen nem használnám, de így létre tudok hozni admin felhasználót üres adatbázisban
        public async Task<IActionResult> CreateAdmin()
        {
            _context.Database.EnsureCreated();

            if (_context.ApplicationUsers.FirstOrDefault(u => u.Email != "admin@admin.eu") == null)
            {
                // Szerepkörök létrehozása ha nem léteznek
                if (!await _roleManager.RoleExistsAsync(SD.Role_Admin))
                {
                    await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                }

                if (!await _roleManager.RoleExistsAsync(SD.Role_User_Breeder))
                {
                    await _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Breeder));
                }

                if (!await _roleManager.RoleExistsAsync(SD.Role_User_Indi))
                {
                    await _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Indi));
                }

                // Admin létrehozása:
                var admin = new ApplicationUser
                {
                    UserName = "admin@admin.eu",
                    Email = "admin@admin.eu",
                    Role = SD.Role_Admin
                };

                await _userManager.CreateAsync(admin, "Pass1?");
                await _userManager.AddToRoleAsync(admin, SD.Role_Admin);

                var adminFromDb = _context.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@admin.eu");
                adminFromDb.LockoutEnabled = false;
                _context.Update(adminFromDb);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
