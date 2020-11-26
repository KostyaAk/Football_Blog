using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using football_blog.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using football_blog.Service;
using System.IO;
using Microsoft.EntityFrameworkCore;
using football_blog.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace football_blog.Controllers
{
    public class PostsController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SiteContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ILogger<PostsController> _logger;
        public PostsController(UserManager<User> userManager, SiteContext context, IWebHostEnvironment appEnvironment, ILogger<PostsController> logger)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index(int? id)
        {
            return View(await _context.Posts.ToListAsync());

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogError("Doesn't exist id. Controller:Post. Action:Details");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                _logger.LogError("Doesn't exist post. Controller:Post. Action:Details");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }

            var user = await _context.Users.FindAsync(post.UserId);
            if (user == null)
            {
                _logger.LogError("Doesn't exist user. Controller:Post. Action:Details");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }
            ViewData["UserName"] = user.UserName;
            List<Comment> comments = new List<Comment>();
            foreach (var item in _context.Comments.Include(s => s.Post).Where(s => s.PostId == post.PostId).ToList())
            {
                item.User = await _context.Users.FindAsync(item.UserId);
                comments.Add(item);
            }

            ViewData["Comment"] = comments;

            return View(post);
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (!user.isBlocked)
            {
                SelectList tags = new SelectList(_context.Tags, "TagID", "TagName");
                ViewBag.Tags = tags;
                ViewData["tags"] = _context.Tags.ToList();
                return View();
            }
            else
            {
                ViewData["Text"] = "Ваша страница заблокирована администратором";
                return View("~/Views/Shared/TextPage.cshtml");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Title,Text,TagId")] Post post, IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                if (uploadedFile != null)
                {
                    string path = "/Files/posts/" + uploadedFile.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    post.Image = path;
                    _context.SaveChanges();
                }
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                post.User = user;
                post.DateTime = DateTime.Now;
                post.Tag = await _context.Tags.FindAsync(post.TagId);
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectPermanent("~/Posts/Index");
            }
            return View(post);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                _logger.LogError("Doesn't exist id. Controller:Post. Action:Edit. id = null");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                _logger.LogError("Doesn't exist post. Controller:Post. Action:Edit. post = null");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }
            var user = await _userManager.FindByIdAsync(post.UserId);
            if (User.Identity.Name.ToString() == user.UserName || User.IsInRole("admin"))
            {
                _logger.LogError("Doesn't exist user. Controller:Post. Action:Edit");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,Text")] Post post, IFormFile uploadedFile)
        {

            if (id != post.PostId)
            {
                _logger.LogError("Doesn't exist id. Controller:Post. Action:Edit");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }
            Post post1 = await _context.Posts.FindAsync(post.PostId);
            if (ModelState.IsValid)
            {
                try
                {
                    if (post.Text != post1.Text && post1 != null)
                        post1.Text = post.Text;
                    if (post.Title != post1.Title && post1 != null)
                        post1.Title = post.Title;
                    if (post.Image != post1.Image && post != null)
                    {
                        string path = "/Files/" + uploadedFile.FileName;
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await uploadedFile.CopyToAsync(fileStream);
                        }
                        post1.Image = path;
                    }
                    _context.Update(post1);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostId))
                    {
                        _logger.LogError("Doesn't exist db. Controller:Post. Action:Edit");
                        return RedirectPermanent("~/Error/Index?statusCode=404");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectPermanent("~/Home/Index");
            }
            return View(post);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogError("Doesn't exist id. Controller:Post. Action:Delete");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                _logger.LogError("Doesn't exist areticle. Controller:Post. Action:Delete");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }

            return View(post);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectPermanent("~/Home/Index");
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
