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



        public ActionResult Add(AssetApplication asset_application)
        {
            var date = asset_application.Date;
            var time = asset_application.Time;
            var assignments = unitOfWork.AssetAssignments.ToList().Where(t => t.Date == date && t.TimeId == time.Id);
            List<Asset> assets = unitOfWork.Assets.ToList().ToList();
            foreach(var assignment in assignments)
            {
                foreach(var assigned_asset in assignment.AssignedAssets)
                {
                    var asset = assets.Find(s => s.Id == assigned_asset.Id);
                    asset.Count -= assigned_asset.Count;
                }
            }
            List<int> app_asset_count_error = new List<int>();
            bool no_error_mark = true;
            foreach(var applicated_asset in asset_application.ApplicatedAssets)
            {
                if( applicated_asset.Count > assets.Find( s => s.Id == applicated_asset.Id ).Count)
                {
                    app_asset_count_error.Add( applicated_asset.Id);
                    no_error_mark = false;
                }
            }

            AssetAssignment asset_assignment = new AssetAssignment
            {
                Date = date,
                Club = asset_application.Club,
                Time = asset_application.Time,
                Applicant = asset_application.Applicant,
                AssignedAssets = new List<AssignedAsset>()
            };

            foreach (var item in asset_application.ApplicatedAssets)
            {
                asset_assignment.AssignedAssets.Add(
                    new AssignedAsset
                    {
                        Id = unitOfWork.GenerateIdFor("AssetBase"),
                        Count = item.Count,
                        Asset = item.Asset
                    });
            }


            if (no_error_mark == true)
            {
                unitOfWork.AssetAssignments.Add(asset_assignment);
                unitOfWork.SaveChanges();
                return RedirectToAction("List");  
            }
            return View();
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