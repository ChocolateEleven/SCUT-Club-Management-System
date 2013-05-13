using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCUTClubManager.Models;
using SCUTClubManager.DAL;
using SCUTClubManager.BusinessLogic;

namespace SCUTClubManager.Controllers
{
    [Authorize]
    public class AssetApplicationController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        //
        // GET: /AssetApplication/

        public ActionResult List(int page_number = 1, string search = "", string search_option = "Club", string order = "Date")
        {
            List<KeyValuePair<string, string>> select_list = new List<KeyValuePair<string, string>>();
            select_list.Add(new KeyValuePair<string, string>("申请人", "Applicant"));
            select_list.Add(new KeyValuePair<string, string>("申请社团","Club"));
            ViewBag.SearchOptions = new SelectList(select_list, "Value", "Key", "Applicant");
            ViewBag.Search = search;
            ViewBag.DateOrderOpt = order == "Date" ? "DateDesc" : "Date";

            var asset_applications = unitOfWork.AssetApplications.ToList();

            if (!String.IsNullOrWhiteSpace(search))
            {
                switch (search_option)
                {
                    case "Applicant":
                        asset_applications = asset_applications.Where(s => s.Applicant.Name.Contains(search));
                        break;
                    case "Club":
                        asset_applications = asset_applications.Where(s => s.Club.MajorInfo.Name.Contains(search));
                        break;
                    default:
                        break;
                }

            }
            var list = QueryProcessor.Query(asset_applications, order_by: order, page_number: page_number, items_per_page: 10);
            return View(list);
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
 
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
        public ActionResult Create(int TimeId, DateTime Date,  int ClubId, int[] item_id, int[] item_borrow_count)
        {
            AssetApplication asset_application = new AssetApplication
            {
                Club = unitOfWork.Clubs.Find(ClubId),
                ApplicantUserName = User.Identity.Name,
                Date = Date,
                Status = "n",
                //Time = unitOfWork.Times.Find(TimeId),
                ApplicatedAssets = new List<ApplicatedAsset>()
            };
            asset_application.Times = new List<Time>();
            asset_application.Times.Add(unitOfWork.Times.Find(TimeId));

            int i = 0;
            foreach(int s in item_id)
            {
                asset_application.ApplicatedAssets.Add( new ApplicatedAsset
                {
                    Asset = new Asset
                    {
                       Name = unitOfWork.Assets.Find(s).Name,
                       Count = item_borrow_count[i]
                    }
                });
                i++;
            }
            unitOfWork.Applications.Add(asset_application);
            unitOfWork.SaveChanges();

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
            //ViewBag.TimeId = new SelectList(unitOfWork.Times.ToList(), "Id", "TimeName", assetapplication.TimeId);
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
            //ViewBag.TimeId = new SelectList(unitOfWork.Times.ToList(), "Id", "TimeName", assetapplication.TimeId);
            ViewBag.SubEventId = new SelectList(unitOfWork.SubEvents.ToList(), "Id", "Title", assetapplication.SubEventId);
            return View(assetapplication);
        }

        public ActionResult Verify(int id, bool is_passed, string reject_reason)
        {
            var asset_application = unitOfWork.AssetApplications.Find(id);
            if (is_passed)
            {
                asset_application.Status = "p";
                unitOfWork.SaveChanges();
                return RedirectToAction("Add", "AssetAssignment", new { id = asset_application.Id });
            }
            asset_application.Status = "f";
            asset_application.RejectReason = new ApplicationRejectReason { Reason = reject_reason };
            unitOfWork.SaveChanges();
            return RedirectToAction("List");
        }

        //
        // GET: /AssetApplication/Delete/5
 
        //public ActionResult Delete(int id)
        //{
        //    AssetApplication assetapplication = unitOfWork.Applications.Find(id) as AssetApplication;
        //    return View(assetapplication);
        //}

        // 
        // POST: /AssetApplication/Delete/5

        [HttpPost]
        public ActionResult Delete(int id)
        {
            AssetApplication assetapplication = unitOfWork.Applications.Find(id) as AssetApplication;
            unitOfWork.Applications.Delete(assetapplication);
            unitOfWork.SaveChanges();
            return Json(new { idToDelete = id, success = true });


            //Location location = unitOfWork.Locations.Find(id);
            //unitOfWork.Locations.Delete(location);
            //unitOfWork.SaveChanges();
            //return Json(new { idToDelete = id, success = true });
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}