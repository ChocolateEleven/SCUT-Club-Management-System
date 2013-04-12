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
                return RedirectToAction("Index");  
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
            ViewBag.timeId = new SelectList(unitOfWork.Times.ToList(),"Id","TimeName");
            return View();
        }

        [HttpPost]
        public ActionResult Calendar(DateTime date,int timeId)
        {
            return RedirectToAction("AvailableAsset", new { date, timeId });
        }

        public ActionResult AvailableAsset(DateTime date,int timeId)
        {
            var assignments = unitOfWork.AssetAssignments.ToList().Where(t => t.Date == date && t.TimeId == timeId);
            List<Asset> assets = unitOfWork.Assets.ToList().ToList();

            foreach (var assignment in assignments)
            {
                foreach (var assigned in assignment.AssignedAssets)
                {
                    var asset = assets.Find(s => s.Id == assigned.AssetId);
                    asset.Count -= assigned.Count;
                }
            }

            IEnumerable<Asset> available_assets = assets.OrderBy(s => s.Name);

            ViewBag.Date = date.ToString("yyyy年MM月dd日");
            ViewBag.Time = unitOfWork.Times.Find(timeId).TimeName;

            return View(available_assets.ToPagedList(1, 10));
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}