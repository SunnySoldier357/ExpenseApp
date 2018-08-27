using ExpenseApp.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ExpenseApp.Controllers
{
    [Route("admin")]
    public class LocationController : Controller
    {
        private readonly ExpenseDBDataContext _db;

        public LocationController(ExpenseDBDataContext db) => _db = db;

        [Route("locations/{message?}")]
        public IActionResult List(string message)
        {
            if (message != null)
                ViewBag.Message = message.Replace('?', '/');

            var locations = _db.Locations
                .OrderBy(l => l.Name)
                .ToArray();

            return View(locations);
        }

        [HttpGet, Route("locations/create")]
        public IActionResult Create() => View();

        [HttpPost, Route("locations/create")]
        public IActionResult Create(Location location)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                _db.Locations.Add(location);
                _db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Database Error! The Name entered is already in the database!");
                return View();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Unknown Error! " + e.Message);
                return View();
            }

            return RedirectToAction("List", "Location", location.Slug);
        }

        [HttpGet, Route("locations/edit/{slug}")]
        public IActionResult Edit(string slug)
        {
            Location location = _db.Locations.FirstOrDefault(l => l.Slug == slug);

            if (null == location)
                return NotFound();

            return View(location);
        }

        [HttpPost, Route("locations/edit/{slug}")]
        public IActionResult Edit(string slug, Location updated)
        {
            Location location = _db.Locations.FirstOrDefault(l => l.Slug == slug);

            if (null == location)
                return NotFound();

            try
            {
                _db.Locations.Remove(location);
                _db.Locations.Add(updated);

                _db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Database Error! The Name entered is already in the database!");
                return View();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Unknown Error! " + e.Message);
                return View();
            }

            return RedirectToAction("List", "Location", updated.Slug);
        }

        [HttpGet, Route("locations/delete/{slug}")]
        public IActionResult Delete(string slug)
        {
            Location location = _db.Locations.FirstOrDefault(l => l.Slug == slug);
            return View(location);
        }

        [HttpPost, Route("locations/delete/{slug}")]
        public IActionResult Delete(string slug, Location deleted)
        {
            Location location = _db.Locations.FirstOrDefault(
                l => l.Slug == slug && l.Name == deleted.Name);

            if (location != null)
            {
                _db.Locations.Remove(location);
                _db.SaveChanges();

                return RedirectToAction("List", "Location", new
                {
                    message = string.Format("The Location \"{0}\" has been successfully deleted.",
                        location.Name.Replace('/', '?'))
                });
            }

            return RedirectToAction("List", "Location", new
            {
                message = string.Format("There was an error in deleting the Location \"{0}\".",
                        location.Name.Replace('/', '?'))
            });
        }
    }
}