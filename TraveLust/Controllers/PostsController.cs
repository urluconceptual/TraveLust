﻿using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TraveLust.Data;
using TraveLust.Models;

namespace TraveLust.Controllers
{
    public class PostsController : Controller
    {

        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IWebHostEnvironment _env;

        public PostsController(
        ApplicationDbContext context,
        IWebHostEnvironment env,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _env = env;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var posts = db.Posts.Include("City");

            ViewBag.Posts = posts;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View();
        }

        public IActionResult Show(int id)
        {
            Post post = db.Posts.Include("City")
                                .Where(p => p.PostId == id).First();

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View(post);
        }

        /*generated by GitHub Copilot*/
        public IActionResult New()
        {
            Post post = new Post();
            post.AllCities = GetAllCities();
            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> New(Post post, IFormFile PostImage)
        {
            post.Date = DateTime.Now;
            post.UserId = _userManager.GetUserId(User);

            var sanitizer = new HtmlSanitizer();

            if (PostImage == null)
            {
                post.AllCities = GetAllCities();
                return View(post);
            }

            if (PostImage.Length > 0)
            {
                // Generam calea de stocare a fisierului
                var storagePath = Path.Combine(
                _env.WebRootPath, // Luam calea folderului wwwroot
                "images", // Adaugam calea folderului images
                PostImage.FileName // Numele fisierului
                );
                // General calea de afisare a fisierului care va fi stocata in  baza de date
                var databaseFileName = "/images/" + PostImage.FileName;
                // Uploadam fisierul la calea de storage
                using (var fileStream = new FileStream(storagePath, FileMode.Create))
                {
                    await PostImage.CopyToAsync(fileStream);
                }
                post.Photo = databaseFileName;
            }


            if (ModelState.IsValid)
            {
                post.Description = sanitizer.Sanitize(post.Description);
                post.Rating = 0;

                db.Posts.Add(post);
                db.SaveChanges();
                TempData["message"] = "Posted!";
                return RedirectToAction("Index");
            }
            else
            {
                post.AllCities = GetAllCities();
                return View(post);
            }
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCities()
        {
            var selectList = new List<SelectListItem>();

            var cities = from c in db.Cities
                         select c;

            foreach (var city in cities)
            {
                selectList.Add(new SelectListItem
                {
                    Value = city.CityId.ToString(),
                    Text = city.CityName.ToString()
                });
            }
            return selectList;
        }
    }
}
