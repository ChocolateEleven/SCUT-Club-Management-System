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
    public class FundController : Controller
    {
        private SCUTClubContext db = new SCUTClubContext();

        //
        // GET: /Fund/

        public ViewResult Index()
        {
            var funddetailses = db.FundDetailses.Include(f => f.Applicant).Include(f => f.Club);
            return View(funddetailses.ToList());
        }

        //
        // GET: /Fund/Details/5

        public ViewResult Details(int id)
        {
            FundDetails funddetails = db.FundDetailses.Find(id);
            return View(funddetails);
        }

        //
        // GET: /Fund/Create

        public ActionResult Create()
        {
            ViewBag.ApplicantUserName = new SelectList(db.Users, "UserName", "Name");
            ViewBag.ClubId = new SelectList(db.Clubs, "Id", "Id");
            return View();
        } 

        //
        // POST: /Fund/Create

        [HttpPost]
        public ActionResult Create(FundDetails funddetails)
        {
            if (ModelState.IsValid)
            {
                db.FundDetailses.Add(funddetails);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.ApplicantUserName = new SelectList(db.Users, "UserName", "Name", funddetails.ApplicantUserName);
            ViewBag.ClubId = new SelectList(db.Clubs, "Id", "Id", funddetails.ClubId);
            return View(funddetails);
        }
        
        //
        // GET: /Fund/Edit/5
 
        public ActionResult Edit(int id)
        {
            FundDetails funddetails = db.FundDetailses.Find(id);
            ViewBag.ApplicantUserName = new SelectList(db.Users, "UserName", "Name", funddetails.ApplicantUserName);
            ViewBag.ClubId = new SelectList(db.Clubs, "Id", "Id", funddetails.ClubId);
            return View(funddetails);
        }

        //
        // POST: /Fund/Edit/5

        [HttpPost]
        public ActionResult Edit(FundDetails funddetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(funddetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicantUserName = new SelectList(db.Users, "UserName", "Name", funddetails.ApplicantUserName);
            ViewBag.ClubId = new SelectList(db.Clubs, "Id", "Id", funddetails.ClubId);
            return View(funddetails);
        }

        //
        // GET: /Fund/Delete/5
 
        public ActionResult Delete(int id)
        {
            FundDetails funddetails = db.FundDetailses.Find(id);
            return View(funddetails);
        }

        //
        // POST: /Fund/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            FundDetails funddetails = db.FundDetailses.Find(id);
            db.FundDetailses.Remove(funddetails);
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