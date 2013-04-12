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
    public class AssetAssignmentController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        //
        // GET: /AssetAssignment/ 

        public ActionResult Index()
        {
            return RedirectToAction("List");     
        }

        public ViewResult List()
        {
            var assetassignments = unitOfWork.AssetAssignments.Include(a => a.Time).Include(a => a.Club).Include(a => a.Applicant);
            return View(assetassignments.ToList());
        }

        //
        // GET: /AssetAssignment/Details/5

        public ViewResult Details(int id)
        {
            AssetAssignment assetassignment = unitOfWork.AssetAssignments.Find(id);
            return View(assetassignment);
        }

        //
        // GET: /AssetAssignment/Create

        public ActionResult Create(DateTime date,int timeId)
        {
            ViewBag.Date = date;
            ViewBag.Time = unitOfWork.Times.Find(timeId);
            ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "Name");
            return View();
        } 

        //
        // POST: /AssetAssignment/Create

        [HttpPost]
        public ActionResult Create(AssetAssignment assetassignment,DateTime date,int timeId)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.AssetAssignments.Add(assetassignment);
                unitOfWork.SaveChanges();
                return RedirectToAction("List");  
            }
            ViewBag.Time = unitOfWork.Times.Find(timeId);
            ViewBag.Date = date;
            ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "Name", assetassignment.ClubId);
            return View(assetassignment);
        }
        
        //
        // GET: /AssetAssignment/Edit/5
 
        public ActionResult Edit(int id)
        {
            AssetAssignment assetassignment = unitOfWork.AssetAssignments.Find(id);
            ViewBag.TimeId = new SelectList(unitOfWork.Times.ToList(), "Id", "TimeName", assetassignment.TimeId);
            ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "Id", assetassignment.ClubId);
            ViewBag.ApplicantUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Name", assetassignment.ApplicantUserName);
            return View(assetassignment);
        }

        //
        // POST: /AssetAssignment/Edit/5

        [HttpPost]
        public ActionResult Edit(AssetAssignment assetassignment)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.AssetAssignments.Update(assetassignment);
                unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TimeId = new SelectList(unitOfWork.Times.ToList(), "Id", "TimeName", assetassignment.TimeId);
            ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "Id", assetassignment.ClubId);
            ViewBag.ApplicantUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Name", assetassignment.ApplicantUserName);
            return View(assetassignment);
        }

        //
        // GET: /AssetAssignment/Delete/5
 
        public ActionResult Delete(int id)
        {
            AssetAssignment assetassignment = unitOfWork.AssetAssignments.Find(id);
            return View(assetassignment);
        }

        //
        // POST: /AssetAssignment/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            AssetAssignment assetassignment = unitOfWork.AssetAssignments.Find(id);
            unitOfWork.AssetAssignments.Delete(assetassignment);
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