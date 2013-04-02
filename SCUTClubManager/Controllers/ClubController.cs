using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SCUTClubManager.Models;
using SCUTClubManager.DAL;
using SCUTClubManager.BusinessLogic;
using SCUTClubManager.Helpers;

namespace SCUTClubManager.Controllers
{ 
    public class ClubController : Controller
    {
        private SCUTClubContext db = new SCUTClubContext();

        //
        // GET: /Club/
        [Authorize]
        public ActionResult Index()
        {
            bool is_recruit_enabled = ConfigurationManager.IsRecruitEnabled;

            ViewBag.EnableRecruitText = is_recruit_enabled ? "关闭社团招新" : "开放社团招新";
            ViewBag.EnableRecruitValue = !is_recruit_enabled;

            if (!Roles.Provider.IsUserInRole(User.Identity.Name, "社联"))
            {
                RedirectToAction("List", new { page_number = 1, order = "", search = "", search_option = "" });
            }

            return View();
        }

        [Authorize(Roles = "社联")]
        [HttpPost]
        public ActionResult EnableRecruit(bool is_enabling)
        {
            ConfigurationManager.IsRecruitEnabled = is_enabling;
            bool result = !ConfigurationManager.IsRecruitEnabled;

            return Json(new { text = ConfigurationManager.IsRecruitEnabled ? "关闭社团招新" : "开放社团招新", value = result });
        }

        public ActionResult List(int page_number, string order, string search, string search_option)
        {
            var clubs = db.Clubs.Include(c => c.Info);
            return View(clubs.ToList());
        }

        //
        // GET: /Club/Details/5

        public ViewResult Details(int id)
        {
            Club club = db.Clubs.Find(id);
            return View(club);
        }

        //
        // GET: /Club/Create

        public ActionResult Create()
        {
            ViewBag.ClubInfoId = new SelectList(db.ClubInfos, "Id", "Name");
            return View();
        } 

        //
        // POST: /Club/Create

        [HttpPost]
        public ActionResult Create(Club club)
        {
            if (ModelState.IsValid)
            {
                db.Clubs.Add(club);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.ClubInfoId = new SelectList(db.ClubInfos, "Id", "Name", club.ClubInfoId);
            return View(club);
        }
        
        //
        // GET: /Club/Edit/5
 
        public ActionResult Edit(int id)
        {
            Club club = db.Clubs.Find(id);
            ViewBag.ClubInfoId = new SelectList(db.ClubInfos, "Id", "Name", club.ClubInfoId);
            return View(club);
        }

        //
        // POST: /Club/Edit/5

        [HttpPost]
        public ActionResult Edit(Club club)
        {
            if (ModelState.IsValid)
            {
                db.Entry(club).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClubInfoId = new SelectList(db.ClubInfos, "Id", "Name", club.ClubInfoId);
            return View(club);
        }

        //
        // GET: /Club/Delete/5
 
        public ActionResult Delete(int id)
        {
            Club club = db.Clubs.Find(id);
            return View(club);
        }

        //
        // POST: /Club/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Club club = db.Clubs.Find(id);
            db.Clubs.Remove(club);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}