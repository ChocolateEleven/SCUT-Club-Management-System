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
    public class AssetAssignmentController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        //
        // GET: /AssetAssignment/ 

        public ActionResult Index()
        {
            return RedirectToAction("List");     
        }

        public ViewResult List(int page_number = 1, string search = "", string search_option = "Club", string order = "Date")
        {
            List<KeyValuePair<string, string>> select_list = new List<KeyValuePair<string, string>>();
            select_list.Add(new KeyValuePair<string,string>("申请人", "Applicant"));
            select_list.Add(new KeyValuePair<string, string>("申请社团", "Club"));
            ViewBag.SearchOptions = new SelectList(select_list, "Value", "Key", "Applicant");
            ViewBag.Search = search;
            ViewBag.DateOrderOpt = order == "Date" ? "DateDesc" : "Date";

            var asseta_ssignments = unitOfWork.AssetAssignments.ToList();

            if (!String.IsNullOrWhiteSpace(search))
            {
                switch (search_option)
                {
                    case "Applicant":
                        asseta_ssignments = asseta_ssignments.Where(s => s.Applicant.Name.Contains(search));
                        break;
                    case "Club":
                        asseta_ssignments = asseta_ssignments.Where(s => s.Club.MajorInfo.Name.Contains(search));
                        break;
                    default:
                        break;
                }
            }

            var list = QueryProcessor.Query(asseta_ssignments, order_by: order, page_number: page_number, items_per_page: 2);

            return View(list);
        }

        //
        // GET: /AssetAssignment/Details/5

        public ViewResult Details(int id)
        {
            AssetAssignment assetassignment = unitOfWork.AssetAssignments.Find(id);
            return View(assetassignment);
        }



        public ActionResult Add(int id)
        {
            Dictionary<int,int> min_avail_asset_count = new Dictionary<int,int>(); //能用的物资数量
            foreach(var item in unitOfWork.Assets.ToList())
            {
                min_avail_asset_count.Add(item.Id,Int32.MaxValue);
            }
            var asset_application = unitOfWork.AssetApplications.Find(id);
            var date = asset_application.Date;
            List<Asset> assets = unitOfWork.Assets.ToList().ToList();
            foreach (var apptime in asset_application.Times)
            {
                //所有跟申请时间相关的assingment
                var assignments = unitOfWork.AssetAssignments.ToList().Where(t => t.Date == date && t.Times.Contains(apptime));
                assets = unitOfWork.Assets.ToList().ToList();
                foreach (var assignment in assignments)
                {
                    foreach (var assigned_asset in assignment.AssignedAssets)
                    {
                        var asset = assets.Find(s => s.Id == assigned_asset.AssetId);
                        asset.Count -= assigned_asset.Count;
                    }
                }
                foreach (var asset in assets)
                {
                    if (asset.Count < min_avail_asset_count[asset.Id])
                    {
                        min_avail_asset_count[asset.Id] = asset.Count;
                    }
                }
            }
            assets = unitOfWork.Assets.ToList().ToList();
            foreach (var asset in assets)
            {
                asset.Count = min_avail_asset_count[asset.Id];
            }



            List<int> app_asset_count_error = new List<int>();
            bool no_error_mark = true;
            foreach(var applicated_asset in asset_application.ApplicatedAssets)
            {
                if( applicated_asset.Count > assets.Find( s => s.Id == applicated_asset.AssetId ).Count)
                {
                    app_asset_count_error.Add( applicated_asset.Id);
                    //申请数量超出可用数量
                    no_error_mark = false;
                }
            }

            AssetAssignment asset_assignment = new AssetAssignment
            {
                Date = date,
                Club = asset_application.Club,
                Applicant = asset_application.Applicant,
                AssignedAssets = new List<AssignedAsset>()
            };
            foreach (var time in asset_application.Times)
            {
                asset_assignment.Times.Add(time);
            }

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
                return RedirectToAction("List","AssetApplication");  
            }

            return RedirectToAction("SetRejectReason", new { enouthAsset = false, application_id = id});
        }

        public ActionResult SetRejectReason(int application_id, bool enoughAsset = true)
        {
            ViewBag.application_id = application_id;
            ViewBag.enoughAsset = enoughAsset;
            if (enoughAsset)
            {
                ViewBag.Reason = ""; 
            }
            else
            {
                ViewBag.Reason = "物资不足，请更改日期后再次申请";
            }
            return View();
        }

        [HttpPost]
        [ActionName("SetRejectReason")]
        public ActionResult SetRejectReason2(ApplicationRejectReason Reason, int application_id)
        {
            if (Reason != null)
            {
                unitOfWork.AssetApplications.Find(application_id).RejectReason = Reason;
            }
            return RedirectToAction("List","AssetApplication");
        }

        //
        // GET: /AssetAssignment/Edit/5
 
        //public ActionResult Edit(int id)
        //{
        //    AssetAssignment assetassignment = unitOfWork.AssetAssignments.Find(id);
        //    ViewBag.TimeId = new SelectList(unitOfWork.Times.ToList(), "Id", "TimeName", assetassignment.TimeId);
        //    ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "Id", assetassignment.ClubId);
        //    ViewBag.ApplicantUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Name", assetassignment.ApplicantUserName);
        //    return View(assetassignment);
        //}

        //
        // POST: /AssetAssignment/Edit/5

        //[HttpPost]
        //public ActionResult Edit(AssetAssignment assetassignment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        unitOfWork.AssetAssignments.Update(assetassignment);
        //        unitOfWork.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.TimeId = new SelectList(unitOfWork.Times.ToList(), "Id", "TimeName", assetassignment.TimeId);
        //    ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "Id", assetassignment.ClubId);
        //    ViewBag.ApplicantUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Name", assetassignment.ApplicantUserName);
        //    return View(assetassignment);
        //}

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