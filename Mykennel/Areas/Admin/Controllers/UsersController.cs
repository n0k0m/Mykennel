using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mykennel.Data;
using Mykennel.Models.ViewModels;
using Mykennel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mykennel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Index()
        {
            var usersVM = (from u in _context.ApplicationUsers
                           select new UserVM { ApplicationUser = u}).ToList();

            foreach (var userVM in usersVM)
            {
                userVM.Kennel = _context.Kennels.FirstOrDefault(k => k.ApplicationUserId == userVM.ApplicationUser.Id);
                var role = await _userManager.GetRolesAsync(userVM.ApplicationUser);
                userVM.ApplicationUser.Role = role[0];
            }

            return View(usersVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] string id)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            // ha nem volt ilyen user
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }
            if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
            {
                // akkor éppen le van zárva és feloldjuk
                user.LockoutEnd = DateTime.Now;
            }
            else
            {
                user.LockoutEnd = DateTime.Now.AddYears(100);
            }
            _context.ApplicationUsers.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Promote(string id)
        {
            var users = _context.ApplicationUsers.ToList();
            var user = users.FirstOrDefault(u => u.Id == id);
            var userRole = await _userManager.GetRolesAsync(user);
            user.Role = userRole[0];
            if (user == null)
            {
                return NotFound();
            }

            if(user.Role == SD.Role_User_Indi)
            {
                await _userManager.RemoveFromRoleAsync(user, SD.Role_User_Indi);
                await _userManager.AddToRoleAsync(user, SD.Role_User_Breeder);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}