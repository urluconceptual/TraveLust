﻿using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;
using TraveLust.Data;
using TraveLust.Models;

namespace TraveLust.Controllers
{
    public class CitiesController : Controller
    {
        private readonly ApplicationDbContext db;

        public CitiesController(ApplicationDbContext context)
        {
            db = context;
        }

        // display all cities
        public ActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }


            var cities = from city in db.Cities
                             orderby city.CityName
                             select city;
            ViewBag.Cities = cities;
            return View();
        }


        // adding a new city
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(City city)
        {

            if (ModelState.IsValid)
            {
                // code generated by GitHub Copilot
                db.Cities.Add(city);
                db.SaveChanges();
                TempData["message"] = "City added!";
                return RedirectToAction("Index", "Posts");
            }
            else
            {
                return View(city);
            }
        }


    }
}
