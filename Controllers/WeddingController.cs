using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Identity;

namespace WeddingPlanner.Controllers
{
    public class WeddingController : Controller
    {
        private Context _context;

        public WeddingController(Context context)
        {
            _context = context;
        }

        private User ActiveUser // creates a new User instance using the id of the logged in user
        {
            get{ return _context.users.Where(u => u.UserId == HttpContext.Session.GetInt32("id")).FirstOrDefault();} // returns one user object where UserId matches session's
        }

        [HttpGet]
        [Route("dashboard")]
        public IActionResult Index() // once the user successfully logs in
        {
            if(ActiveUser == null)
                return RedirectToAction("Index", "Home");

            User thisUser = _context.users.Where(u => u.UserId == HttpContext.Session.GetInt32("id")).Include(u => u.Rsvp).FirstOrDefault();
            ViewBag.UserInfo = thisUser;

            List<Wedding> weddings = _context.weddings.Include(w => w.Rsvp).ToList();

            return View(weddings);
        }

        [HttpGet]
        [Route("newwedding")]
        public IActionResult NewWedding()
        {
            if (ActiveUser == null)
                return RedirectToAction("Index", "Home");

            ViewBag.UserInfo = ActiveUser;
            return View();
        }

        [HttpPost]
        [Route("addwedding")]
        public IActionResult AddWedding(AddWedding wedding)
        {
            if (ActiveUser == null)
                return RedirectToAction("Index", "Home");

            if(ModelState.IsValid)
            {
                // add to db
                Wedding Wedding = new Wedding
                {
                    CreatedBy = ActiveUser.UserId,
                    WedderOne = wedding.WedderOne,
                    WedderTwo = wedding.WedderTwo,
                    Date = wedding.Date,
                    Address = wedding.Address,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _context.weddings.Add(Wedding);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserInfo = ActiveUser;
            return View("NewWedding");
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult Delete(int WeddingId)
        {
            if (ActiveUser == null)
                return RedirectToAction("Index", "Home");

            Wedding ToDelete = _context.weddings.SingleOrDefault(wedding => wedding.WeddingId == WeddingId);
            _context.weddings.Remove(ToDelete);
            _context.SaveChanges();

            ViewBag.UserInfo = ActiveUser;
            List<Wedding> weddings = _context.weddings.ToList();
            return View("Index", weddings);
        }

        [HttpPost]
        [Route("rsvp")]
        public IActionResult Rsvp(int WeddingId)
        {
            if (ActiveUser == null)
                return RedirectToAction("Index", "Home");

            Rsvp rsvp = new Rsvp
            {
                UserId = ActiveUser.UserId,
                WeddingId = WeddingId
            };
            _context.rsvps.Add(rsvp);
            _context.SaveChanges();

            ViewBag.UserInfo = ActiveUser;
            List<Wedding> weddings = _context.weddings.ToList();
            return RedirectToAction("Index", weddings);
        }

        [HttpPost]
        [Route("unrsvp")]
        public IActionResult UnRsvp(int WeddingId)
        {
            if (ActiveUser == null)
                return RedirectToAction("Index", "Home");

            Rsvp ToDelete = _context.rsvps.SingleOrDefault(rsvp => rsvp.WeddingId == WeddingId && rsvp.UserId == ActiveUser.UserId);
            _context.rsvps.Remove(ToDelete);
            _context.SaveChanges();

            ViewBag.UserInfo = ActiveUser;
            List<Wedding> weddings = _context.weddings.ToList();
            return RedirectToAction("Index", weddings);
        }

        [HttpGet]
        [Route("wedding/{WeddingId}")]
        public IActionResult ShowWedding(int WeddingId) // once the user successfully logs in
        {
            if (ActiveUser == null)
                return RedirectToAction("Index", "Home");
                
            ViewBag.wedding = _context.weddings.Where(w => w.WeddingId == WeddingId).Include(w => w.Rsvp).ThenInclude(r => r.User).SingleOrDefault();
            return View("ShowWedding");
        }
    }
}