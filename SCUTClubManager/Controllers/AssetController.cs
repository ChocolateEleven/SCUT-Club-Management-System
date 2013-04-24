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
using PagedList;

namespace SCUTClubManager.Controllers
{ 
    public class AssetController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        //
        // GET: /Asset/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(int page_number = 1, string search = "")
        {
             ViewBag.Search = search;
           var  asset =  QueryProcessor.Query<Asset>(unitOfWork.Assets.ToList(),
               filter: t => t.Name.Contains(search), order_by: "Name", page_number: page_number, items_per_page: 2);
            return View(asset);
        }

        //
        // GET: /Asset/Details/5

        public ViewResult Details(int id)
        {
            Asset asset = unitOfWork.Assets.Find(id);
            return View(asset);
        }

        //
        // GET: /Asset/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Asset/Create

        [HttpPost]
        public ActionResult Create(Asset asset)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Assets.Add(asset);
                unitOfWork.SaveChanges();
                return RedirectToAction("List");  
            }

            return View(asset);
        }
        
        //
        // GET: /Asset/Edit/5
 
        public ActionResult Edit(int id)
        {
            Asset asset = unitOfWork.Assets.Find(id);
            return View(asset);
        }

        //
        // POST: /Asset/Edit/5

        [HttpPost]
        public ActionResult Edit(Asset asset)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Assets.Update(asset);
                unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(asset);
        }

        //
        // GET: /Asset/Delete/5
 
        public ActionResult Delete(int id)
        {
            Asset asset = unitOfWork.Assets.Find(id);
            return View(asset);
        }

        //
        // POST: /Asset/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Asset asset = unitOfWork.Assets.Find(id);
            unitOfWork.Assets.Delete(asset);
            unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Calendar()
        {
            ViewBag.Times = new SelectList(unitOfWork.Times.ToList(), "Id", "TimeName");
            return View();
        }

        //[HttpPost]
        //public ActionResult Calendar(DateTime date,int[] time_ids)
        //{
        //    ViewBag.Times = new SelectList(unitOfWork.Times.ToList(), "Id", "TimeName");
        //    return RedirectToAction("AvailableAsset", new { date, });
        //}

        public ActionResult AssignedAsset(DateTime date, int[] time_ids)
        {

            List<AssignedAsset> assigned_assets = new List<AssignedAsset>();
            foreach (var time_id in time_ids)
            {
                var a = unitOfWork.AssetAssignments.ToList().ToList();
                var assignments = unitOfWork.AssetAssignments.ToList().Where(t => t.Date == date && t.Times.Any(s => s.Id == time_id)).ToList();
                foreach (var assignment in assignments)
                {
                    foreach (var assigned_asset in assignment.AssignedAssets)
                    {
                        assigned_assets.Add(assigned_asset);
                    }
                }
            }

            IEnumerable<AssignedAsset>  list = assigned_assets.OrderBy(s => s.Asset.Name);
            ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "MajorInfo.Name");
            ViewBag.SubEventId = new SelectList(unitOfWork.SubEvents.ToList(), "Id", "Title");
            ViewBag.Date = date.ToString("yyyy年MM月dd日");

            List<Time> temp_time = new List<Time>();
            foreach (var time_id in time_ids)
            {
                temp_time.Add(unitOfWork.Times.Find(time_id));
            }
            ViewBag.Times = temp_time;
            ViewBag.time_ids = time_ids;

            return View(list.ToPagedList(1, 10));
        }

        public ActionResult AvailableAsset(DateTime date,int[] time_ids)
        {
            
            Dictionary<int,int> min_avail_asset_count = new Dictionary<int,int>(); //能用的物资数量
            Dictionary<int, int> asset_count = new Dictionary<int, int>();
            foreach(var item in unitOfWork.Assets.ToList())
            {
                min_avail_asset_count.Add(item.Id,Int32.MaxValue);
                asset_count.Add(item.Id, item.Count);
            }
            List<Asset> assets = unitOfWork.Assets.ToList().ToList();
            foreach (var time_id in time_ids)
            {
                foreach (var asset in assets)
                {
                    asset.Count = asset_count[asset.Id];
                }
                var assignments = unitOfWork.AssetAssignments.ToList().Where(t => t.Date == date && t.Times.Any(s => s.Id == time_id));
                

                foreach (var assignment in assignments)
                {
                    foreach (var assigned in assignment.AssignedAssets)
                    {
                        var asset = assets.Find(s => s.Id == assigned.AssetId);
                        asset.Count -= assigned.Count;
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

            IEnumerable<Asset> available_assets = assets.OrderBy(s => s.Name);

            ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "MajorInfo.Name");
            ViewBag.SubEventId = new SelectList(unitOfWork.SubEvents.ToList(), "Id", "Title");
            ViewBag.Date = date.ToString("yyyy年MM月dd日");


            List<Time> temp_time = new List<Time>();
            foreach (var time_id in time_ids)
            {
                temp_time.Add(unitOfWork.Times.Find(time_id));
            }
            ViewBag.Times = temp_time;
            ViewBag.time_ids = time_ids;

            return View(available_assets.ToPagedList(1, 10));
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}