using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class KennelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public KennelsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Admin/Kennels
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Kennels.Include(k => k.ApplicationUser).Include(k => k.Country);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Kennels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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

        // GET: Admin/Kennels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
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

        // POST: Admin/Kennels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
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
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        /*
        // POST: Admin/Kennels1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kennel = await _context.Kennels.FindAsync(id);
            _context.Kennels.Remove(kennel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */
        private bool KennelExists(int id)
        {
            return _context.Kennels.Any(e => e.KennelId == id);
        }
    }
}
