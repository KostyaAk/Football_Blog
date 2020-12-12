using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using football_blog.Models;
using football_blog.Service;

namespace football_blog.Controllers
{
    public class ClubsController : Controller
    {
        private readonly SiteContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ImageService _imageService;
        public ClubsController(SiteContext context, IWebHostEnvironment appEnvironment, ImageService imageService)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _imageService = imageService;
        }



        public async Task<IActionResult> Index()
        {
            return View(await _context.Clubs.ToListAsync());
        }



        public async Task<IActionResult> Details(int id)
        {
            var club = await _context.Clubs
                .FirstOrDefaultAsync(m => m.ClubId == id);
            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }


        public IActionResult Create()
        {
            return View();
        }



        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClubID,ClubName,ClubDescription")] Club club, IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                if (uploadedFile != null && uploadedFile.ContentType.ToLower().Contains("image"))
                {

                    club.ClubImage = await _imageService.SaveImageAsync(uploadedFile, 0);
                    _context.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("Image", "Некорректный формат");
                    return View(club);
                }
                _context.Add(club);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(club);
        }


        public async Task<IActionResult> Edit(int id)
        {

            var club = await _context.Clubs.FindAsync(id);
            if (club == null)
            {
                return NotFound();
            }
            return View(club);
        }

        // POST: Clubs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
         public async Task<IActionResult> Edit(int id, [Bind("ClubId,ClubName,ClubDescription,ClubImage")] Club club, IFormFile uploadedFile)
        {

            Club club1 = await _context.Clubs
                .FirstOrDefaultAsync(m => m.ClubId == id);
            if (ModelState.IsValid)
            {
                try
                {
                    if (club.ClubDescription != club1.ClubDescription && club1.ClubDescription != null)
                        club1.ClubDescription = club.ClubDescription;
                    if (club.ClubName != club1.ClubName && club1.ClubName != null)
                        club1.ClubName = club.ClubName;
                    if (uploadedFile != null && uploadedFile.ContentType.ToLower().Contains("image"))
                    {
                        club1.ClubImage = await _imageService.SaveImageAsync(uploadedFile, 0);
                    }
                    _context.Update(club1);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClubExists(club.ClubId))
                    {
                        return RedirectPermanent("~/Error/Index?statusCode=404");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectPermanent("~/Clubs");
            }
            return View(club);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var club = await _context.Clubs
                .FirstOrDefaultAsync(m => m.ClubId == id);
            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var club = await _context.Clubs.FindAsync(id);
            _context.Clubs.Remove(club);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClubExists(int id)
        {
            return _context.Clubs.Any(e => e.ClubId == id);
        }
    }
}
