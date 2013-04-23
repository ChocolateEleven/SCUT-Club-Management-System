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
        public ActionResult Create( Location location,int[] weekday, int[] time_id)
        {
            if (ModelState.IsValid)
            {
                if (weekday != null)
                {
                    location.UnAvailableTimes = new List<LocationUnavailableTime>();
                    int i=0;
                    foreach (var week_day in weekday)
                    {
                        location.UnAvailableTimes.Add(new LocationUnavailableTime
                        {
                            Time = unitOfWork.Times.Find(time_id[i]),
                            WeekDayId = week_day
                        });
                    i++;
                    }
                }
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
            ViewBag.Time = new SelectList(unitOfWork.Times.ToList(), "Id", "TimeName");
            ViewBag.UnavailableTimes = location.UnAvailableTimes;
            return View(location);
        }

        //
        // POST: /Location/Edit/5

        [HttpPost]
        public ActionResult Edit(Location location,int[] time_id, int[] weekday)
        {
            location = unitOfWork.Locations.Find(location.Id);

            if (ModelState.IsValid)
            {

                if (time_id != null)
                {
                    for (int i = 0; i < time_id.Length; ++i)
                    {
                        if (location.UnAvailableTimes.All(t => t.TimeId != time_id[i] || t.WeekDayId != weekday[i]))
                        {
                            var temp_time = new LocationUnavailableTime
                                {
                                    WeekDayId = weekday[i],
                                    Time = unitOfWork.Times.Find(time_id[i])
                                };

                            location.UnAvailableTimes.Add(temp_time);
                        }
                    }
                    
                }
                unitOfWork.Locations.Update(location);
                unitOfWork.SaveChanges();

                ViewBag.Time = unitOfWork.Times.ToList();
                return RedirectToAction("Index");
            }
            ViewBag.Time = new SelectList(unitOfWork.Times.ToList(), "Id", "TimeName");
            ViewBag.UnavailableTimes = location.UnAvailableTimes;
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
            var weekdayOfDate = date.DayOfWeek.ToString();
            int weekday = 0;
            switch (weekdayOfDate.ToLower())
            {
                case "monday":
                    weekday = 1;
                    break;
                case "tuesday":
                    weekday = 2;
                    break;
                case "wednesday":
                    weekday = 3;
                    break;
                case "thursday":
                    weekday = 4;
                    break;
                case "friday":
                    weekday = 5;
                    break;
                case "saturday":
                    weekday = 6;
                    break;
                case "sunday":
                    weekday = 7;
                    break;
                default:
                    break;
            }

            //删除被占用的场地
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

            //删除当天当时间段不可用的场地
            foreach (var location in locations)
            {
                foreach(var unavailabletime in location.UnAvailableTimes)
                {
                    if (unavailabletime.TimeId == timeId && unavailabletime.WeekDayId == weekday)
                    {
                        locations.Remove(location);
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