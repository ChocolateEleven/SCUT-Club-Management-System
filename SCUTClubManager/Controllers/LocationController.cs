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
using SCUTClubManager.Models.View_Models;

namespace SCUTClubManager.Controllers
{ 
    public class LocationController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        //
        // GET: /Location/
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult List()
        {
            return View(unitOfWork.Locations.ToList());
        }

        //
        // GET: /Location/Details/5

        public ViewResult Details(int id)
        {
            Location location = unitOfWork.Locations.Find(id);
            return View(location);
        }

        //
        // GET: /Location/Create

        public ActionResult Create()
        {
            ViewBag.Time = new SelectList(unitOfWork.Times.ToList(),"Id","TimeName");
            return View();
        } 

        //
        // POST: /Location/Create

        [HttpPost]
        public ActionResult Create(Location location)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Locations.Add(location);
                unitOfWork.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(location);
        }
        
        //
        // GET: /Location/Edit/5
 
        public ActionResult Edit(int id)
        {
            Location location = unitOfWork.Locations.Find(id);
            return View(location);
        }

        //
        // POST: /Location/Edit/5

        [HttpPost]
        public ActionResult Edit(Location location)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Locations.Update(location);
                unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(location);
        }

        //
        // GET: /Location/Delete/5
 
        public ActionResult Delete(int id)
        {
            Location location = unitOfWork.Locations.Find(id);
            return View(location);
        }

        //
        // POST: /Location/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Location location = unitOfWork.Locations.Find(id);
            unitOfWork.Locations.Delete(location);
            unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Calendar()
        {
            ViewBag.timeId = new SelectList(unitOfWork.Times.ToList(), "Id", "TimeName");
            return View();
        }

        [HttpPost]
        public ActionResult Calendar(DateTime date, int timeId)
        {
            return RedirectToAction("AvailableLocation", new { date, timeId });
        }

        public ActionResult AvailableLocation(DateTime date, int timeId)
        {
            var assigned_locations = unitOfWork.LocationAssignments.ToList().Where(s => s.Date == date && s.TimeId == timeId);
            List<Location> locations = unitOfWork.Locations.ToList().ToList();

            foreach (var assigned_location in assigned_locations)
            {
                foreach (var location in locations)
                {
                    if (location.Id == assigned_location.Id)
                    {
                        locations.Remove(locations.Find(s => s.Id == location.Id));
                    }
                }
            }

            IEnumerable<Location> available_locations = locations.OrderBy(s => s.Name);

            ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "MajorInfo.Name");
            ViewBag.SubEventId = new SelectList(unitOfWork.SubEvents.ToList(), "Id", "Title");
            ViewBag.Date = date.ToString("yyyy年MM月dd日");
            var time = unitOfWork.Times.Find(timeId);
            ViewBag.Time = time.TimeName;
            ViewBag.TimeId = time.Id;

            return View(available_locations.ToPagedList(1,10));
        }

        public ActionResult AssignedLocation(DateTime date, int timeId)
        {
            var assignments = unitOfWork.LocationAssignments.ToList().Where(s => s.Date == date && s.TimeId == timeId);
            //List<Location> locations = new List<Location>();
            List<AssignedLocationViewModel> assignedLocation = new List<AssignedLocationViewModel>();
            foreach(var location_assignment in assignments)
            {
                foreach(var location in location_assignment.Locations)
                {
                    assignedLocation.Add(new AssignedLocationViewModel{
                        Location = location,
                        Club = location_assignment.Club,
                        Applicant = location_assignment.Applicant
                    });
                }
            }



            IEnumerable<AssignedLocationViewModel> list = assignedLocation.OrderBy(s => s.Location.Name);

            ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "MajorInfo.Name");
            ViewBag.SubEventId = new SelectList(unitOfWork.SubEvents.ToList(), "Id", "Title");
            ViewBag.Date = date.ToString("yyyy年MM月dd日");
            var time = unitOfWork.Times.Find(timeId);
            ViewBag.Time = time.TimeName;
            ViewBag.TimeId = time.Id;

            return View(list.ToPagedList(1,10));
        }


        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}