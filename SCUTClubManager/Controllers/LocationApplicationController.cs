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
    public class LocationApplicationController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        //
        // GET: /LocationApplication/

        public ViewResult List(int page_number = 1, string search = "", string search_option = "Club", string order = "Date")
        {
            List<KeyValuePair<string, string>> select_list = new List<KeyValuePair<string, string>>();
            select_list.Add(new KeyValuePair<string, string>("申请人", "Applicant"));
            select_list.Add(new KeyValuePair<string, string>("申请社团", "Club"));
            ViewBag.SearchOptions = new SelectList(select_list, "Value", "Key", "Applicant");
            ViewBag.Search = search;
            ViewBag.DateOrderOpt = order == "Date" ? "DateDesc" : "Date";

            var location_applications = unitOfWork.LocationApplications.ToList();

            if (!String.IsNullOrWhiteSpace(search))
            {
                switch (search_option)
                {
                    case "Applicant":
                        location_applications = location_applications.Where(s => s.Applicant.Name.Contains(search));
                        break;
                    case "Club":
                        location_applications = location_applications.Where(s => s.Club.MajorInfo.Name.Contains(search));
                        break;
                    default:
                        break;
                }

            }
            var list = QueryProcessor.Query(location_applications, order_by: order, page_number: page_number, items_per_page: 2);
            return View(list);
        }

        //
        // GET: /LocationApplication/Details/5

        public ViewResult Details(int id)
        {
            LocationApplication locationapplication = unitOfWork.Applications.Find(id) as LocationApplication;
            return View(locationapplication);
        }

        //
        // GET: /LocationApplication/Create

        public ActionResult Create()
        {
            ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "Id");
            ViewBag.ApplicantUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Name");
            ViewBag.Id = new SelectList(unitOfWork.ApplicationRejectReasons.ToList(), "ApplicationId", "Reason");
            ViewBag.Times = new SelectList(unitOfWork.Times.ToList(), "Id", "TimeName");
            ViewBag.SubEventId = new SelectList(unitOfWork.SubEvents.ToList(), "Id", "Title");
            return View();
        } 

        //
        // POST: /LocationApplication/Create

        [HttpPost]
        public ActionResult Create(int[] time_ids, DateTime date ,int[] locationId,int ClubId, int? SubEventId)
        {
            LocationApplication location_application = new LocationApplication();
            foreach (var time_id in time_ids)
            {
                location_application.Times.Add(unitOfWork.Times.Find(time_id));
            }
            location_application.Date = date;
            if (SubEventId != null)
                location_application.SubEvent = unitOfWork.SubEvents.Find(SubEventId);
            location_application.Club = unitOfWork.Clubs.Find(ClubId);
            location_application.Location = new List<Location>();
            foreach (var id in locationId)
            {
                location_application.Location.Add(unitOfWork.Locations.Find(id));
            }
            location_application.ApplicantUserName = User.Identity.Name;
            location_application.Status = Application.NOT_VERIFIED;
            unitOfWork.Applications.Add(location_application);
            unitOfWork.SaveChanges();
            return RedirectToAction("Index", "Location");
        }
        
        //
        // GET: /LocationApplication/Edit/5
 
        //public ActionResult Edit(int id)
        //{
        //    LocationApplication locationapplication = unitOfWork.Applications.Find(id) as LocationApplication;
        //    ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "Id", locationapplication.ClubId);
        //    ViewBag.ApplicantUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Name", locationapplication.ApplicantUserName);
        //    ViewBag.Id = new SelectList(unitOfWork.ApplicationRejectReasons.ToList(), "ApplicationId", "Reason", locationapplication.Id);
        //    ViewBag.TimeId = new SelectList(unitOfWork.Times.ToList(), "Id", "TimeName", locationapplication.TimeId);
        //    ViewBag.SubEventId = new SelectList(unitOfWork.SubEvents.ToList(), "Id", "Title", locationapplication.SubEventId);
        //    return View(locationapplication);
        //}

        //
        // POST: /LocationApplication/Edit/5

        //[HttpPost]
        //public ActionResult Edit(LocationApplication locationapplication)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        unitOfWork.LocationApplications.Update(locationapplication);
        //        unitOfWork.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "Id", locationapplication.ClubId);
        //    ViewBag.ApplicantUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Name", locationapplication.ApplicantUserName);
        //    ViewBag.Id = new SelectList(unitOfWork.ApplicationRejectReasons.ToList(), "ApplicationId", "Reason", locationapplication.Id);
        //    ViewBag.TimeId = new SelectList(unitOfWork.Times.ToList(), "Id", "TimeName", locationapplication.TimeId);
        //    ViewBag.SubEventId = new SelectList(unitOfWork.SubEvents.ToList(), "Id", "Title", locationapplication.SubEventId);
        //    return View(locationapplication);
        //}

        //
        // GET: /LocationApplication/Delete/5
 
        public ActionResult Delete(int id)
        {
            LocationApplication locationapplication = unitOfWork.Applications.Find(id) as LocationApplication;
            return View(locationapplication);
        }

        //
        // POST: /LocationApplication/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            LocationApplication locationapplication = unitOfWork.Applications.Find(id) as LocationApplication;
            unitOfWork.Applications.Delete(locationapplication);
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