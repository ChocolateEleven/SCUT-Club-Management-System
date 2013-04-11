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
            return RedirectToAction("List");
        }

        public ActionResult List(int page_number = 1, string search = "", string order = "Name")
        {
             ViewBag.Search = search;
           var  asset =  QueryProcessor.Query<Asset>(unitOfWork.Assets.ToList(),
               filter: t => t.Name.Contains(search), order_by: order, page_number: page_number, items_per_page: 2);
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

        public ActionResult Calender()
        {
            ViewBag.timeId = new SelectList(unitOfWork.Times.ToList(),"Id","TimeName");
            return View();
        }

        [HttpPost]
        public ActionResult Calender(DateTime date,int timeId)
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

            IEnumerable<Asset> available_assets = assets.Where(t => t.Count > 0).OrderBy(s => s.Name);

            return View(available_assets.ToPagedList(1, 10));
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}