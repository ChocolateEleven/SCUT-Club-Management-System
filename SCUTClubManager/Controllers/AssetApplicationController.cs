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
        private UnitOfWork unitOfWork = new UnitOfWork();

        //
        // GET: /AssetApplication/

        public ViewResult Index()
        {
            List<AssetApplication> assetApplications = new List<AssetApplication>();
            foreach(var application in unitOfWork.Applications.ToList())
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
            AssetApplication assetapplication = unitOfWork.Applications.Find(id) as AssetApplication;
            return View(assetapplication);
        }

        //
        // GET: /AssetApplication/Create
        [HttpPost]
        public ActionResult Create(string[] item_id, string[] item_name, string[] item_borrow_count)
        {
            
            //ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "Id");
            //ViewBag.ApplicantUserName = User.Identity.Name;
            //ViewBag.Id =
            //ViewBag.TimeId = new SelectList(unitOfWork.Times.ToList(), "Id", "TimeName");
            //ViewBag.SubEventId = new SelectList(unitOfWork.SubEvents.ToList(), "Id", "Title");
            return View();
        } 

        //
        // POST: /AssetApplication/Create

        //[HttpPost]
        //public ActionResult Create(AssetApplication assetapplication)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        unitOfWork.Applications.Add(assetapplication);
        //        unitOfWork.SaveChanges();
        //        return RedirectToAction("Index");  
        //    }

        //    ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "Id", assetapplication.ClubId);
        //    ViewBag.ApplicantUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Name", assetapplication.ApplicantUserName);
        //    ViewBag.Id = new SelectList(unitOfWork.ApplicationRejectReasons.ToList(), "ApplicationId", "Reason", assetapplication.Id);
        //    ViewBag.TimeId = new SelectList(unitOfWork.Times.ToList(), "Id", "TimeName", assetapplication.TimeId);
        //    ViewBag.SubEventId = new SelectList(unitOfWork.SubEvents.ToList(), "Id", "Title", assetapplication.SubEventId);
        //    return View(assetapplication);
        //}
        
        //
        // GET: /AssetApplication/Edit/5
 
        public ActionResult Edit(int id)
        {
            AssetApplication assetapplication = unitOfWork.Applications.Find(id) as AssetApplication;
            ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "Id", assetapplication.ClubId);
            ViewBag.ApplicantUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Name", assetapplication.ApplicantUserName);
            ViewBag.Id = new SelectList(unitOfWork.ApplicationRejectReasons.ToList(), "ApplicationId", "Reason", assetapplication.Id);
            ViewBag.TimeId = new SelectList(unitOfWork.Times.ToList(), "Id", "TimeName", assetapplication.TimeId);
            ViewBag.SubEventId = new SelectList(unitOfWork.SubEvents.ToList(), "Id", "Title", assetapplication.SubEventId);
            return View(assetapplication);
        }

        //
        // POST: /AssetApplication/Edit/5

        [HttpPost]
        public ActionResult Edit(AssetApplication assetapplication)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.AssetApplications.Update(assetapplication);
                unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "Id", assetapplication.ClubId);
            ViewBag.ApplicantUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Name", assetapplication.ApplicantUserName);
            ViewBag.Id = new SelectList(unitOfWork.ApplicationRejectReasons.ToList(), "ApplicationId", "Reason", assetapplication.Id);
            ViewBag.TimeId = new SelectList(unitOfWork.Times.ToList(), "Id", "TimeName", assetapplication.TimeId);
            ViewBag.SubEventId = new SelectList(unitOfWork.SubEvents.ToList(), "Id", "Title", assetapplication.SubEventId);
            return View(assetapplication);
        }

        //
        // GET: /AssetApplication/Delete/5
 
        public ActionResult Delete(int id)
        {
            AssetApplication assetapplication = unitOfWork.Applications.Find(id) as AssetApplication;
            return View(assetapplication);
        }

        //
        // POST: /AssetApplication/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            AssetApplication assetapplication = unitOfWork.Applications.Find(id) as AssetApplication;
            unitOfWork.Applications.Delete(assetapplication);
            unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}