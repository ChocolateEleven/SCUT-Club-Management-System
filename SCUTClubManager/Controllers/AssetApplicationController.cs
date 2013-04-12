using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCUTClubManager.Models;
using SCUTClubManager.DAL;

namespace SCUTClubManager.Controllers
{ 
    public class AssetApplicationController : Controller
    {
        private SCUTClubContext db = new SCUTClubContext();

        //
        // GET: /AssetApplication/

        public ViewResult Index()
        {
            List<AssetApplication> assetApplications = new List<AssetApplication>();
            foreach(var application in db.Applications)
            {
                if (application is AssetApplication)
                {
                    assetApplications.Add(application as AssetApplication);
                }
            }
            return View(assetApplications.ToList());
        }

        //
        // GET: /AssetApplication/Details/5

        public ViewResult Details(int id)
        {
            AssetApplication assetapplication = db.Applications.Find(id) as AssetApplication;
            return View(assetapplication);
        }

        //
        // GET: /AssetApplication/Create

        public ActionResult Create()
        {
            ViewBag.ClubId = new SelectList(db.Clubs, "Id", "Id");
            ViewBag.ApplicantUserName = new SelectList(db.Users, "UserName", "Name");
            ViewBag.Id = new SelectList(db.ApplicationRejectReasons, "ApplicationId", "Reason");
            ViewBag.TimeId = new SelectList(db.Times, "Id", "TimeName");
            ViewBag.SubEventId = new SelectList(db.SubEvents, "Id", "Title");
            return View();
        } 

        //
        // POST: /AssetApplication/Create

        [HttpPost]
        public ActionResult Create(AssetApplication assetapplication)
        {
            if (ModelState.IsValid)
            {
                db.Applications.Add(assetapplication);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.ClubId = new SelectList(db.Clubs, "Id", "Id", assetapplication.ClubId);
            ViewBag.ApplicantUserName = new SelectList(db.Users, "UserName", "Name", assetapplication.ApplicantUserName);
            ViewBag.Id = new SelectList(db.ApplicationRejectReasons, "ApplicationId", "Reason", assetapplication.Id);
            ViewBag.TimeId = new SelectList(db.Times, "Id", "TimeName", assetapplication.TimeId);
            ViewBag.SubEventId = new SelectList(db.SubEvents, "Id", "Title", assetapplication.SubEventId);
            return View(assetapplication);
        }
        
        //
        // GET: /AssetApplication/Edit/5
 
        public ActionResult Edit(int id)
        {
            AssetApplication assetapplication = db.Applications.Find(id) as AssetApplication;
            ViewBag.ClubId = new SelectList(db.Clubs, "Id", "Id", assetapplication.ClubId);
            ViewBag.ApplicantUserName = new SelectList(db.Users, "UserName", "Name", assetapplication.ApplicantUserName);
            ViewBag.Id = new SelectList(db.ApplicationRejectReasons, "ApplicationId", "Reason", assetapplication.Id);
            ViewBag.TimeId = new SelectList(db.Times, "Id", "TimeName", assetapplication.TimeId);
            ViewBag.SubEventId = new SelectList(db.SubEvents, "Id", "Title", assetapplication.SubEventId);
            return View(assetapplication);
        }

        //
        // POST: /AssetApplication/Edit/5

        [HttpPost]
        public ActionResult Edit(AssetApplication assetapplication)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assetapplication).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClubId = new SelectList(db.Clubs, "Id", "Id", assetapplication.ClubId);
            ViewBag.ApplicantUserName = new SelectList(db.Users, "UserName", "Name", assetapplication.ApplicantUserName);
            ViewBag.Id = new SelectList(db.ApplicationRejectReasons, "ApplicationId", "Reason", assetapplication.Id);
            ViewBag.TimeId = new SelectList(db.Times, "Id", "TimeName", assetapplication.TimeId);
            ViewBag.SubEventId = new SelectList(db.SubEvents, "Id", "Title", assetapplication.SubEventId);
            return View(assetapplication);
        }

        //
        // GET: /AssetApplication/Delete/5
 
        public ActionResult Delete(int id)
        {
            AssetApplication assetapplication = db.Applications.Find(id) as AssetApplication;
            return View(assetapplication);
        }

        //
        // POST: /AssetApplication/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            AssetApplication assetapplication = db.Applications.Find(id) as AssetApplication;
            db.Applications.Remove(assetapplication);
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