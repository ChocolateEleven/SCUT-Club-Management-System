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
    [Authorize]
    public class LocationController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        //
        // GET: /Location/
        public ActionResult Index()
        {
            if ( !User.IsInRole("社联") && RoleHelper.GetRoleClub(User.Identity.Name, "会长").Count == 0 )
            {
                return View("PermissionDeniedError");
            }
            return View();
        }


        public ActionResult List(int page_number = 1, string search = "", string search_option = "Name", string order = "Name")
        {
            if ( !User.IsInRole("社联") && RoleHelper.GetRoleClub(User.Identity.Name, "会长").Count == 0 )
            {
                return View("PermissionDeniedError");
            }
            List<KeyValuePair<string, string>> select_list = new List<KeyValuePair<string, string>>();
            select_list.Add(new KeyValuePair<string, string>("场地名", "Name"));
            ViewBag.SearchOptions = new SelectList(select_list, "Value", "Key", "Name");
            ViewBag.Search = search;
            ViewBag.CurrentOrder = order;
            ViewBag.NameOrderOpt = order == "Name" ? "NameDesc" : "Name";


            var polls = unitOfWork.Locations.ToList();
            var list = QueryProcessor.Query(polls, order_by: order, page_number: page_number, items_per_page: 10);

            return View(list);
        }

        //
        // GET: /Location/Details/5

        public ViewResult Details(int id)
        {
            
            if ( !User.IsInRole("社联") && RoleHelper.GetRoleClub(User.Identity.Name, "会长").Count == 0 )
            {
                return View("PermissionDeniedError");
            }
            Location location = unitOfWork.Locations.Find(id);
            return View(location);
        }

        //
        // GET: /Location/Create

        public ActionResult Create()
        {
            if ( !User.IsInRole("社联") && RoleHelper.GetRoleClub(User.Identity.Name, "会长").Count == 0 )
            {
                return View("PermissionDeniedError");
            }
            ViewBag.Time = new SelectList(unitOfWork.Times.ToList(),"Id","TimeName");
            return View();
        } 

        //
        // POST: /Location/Create

        [HttpPost]
        public ActionResult Create( Location location,int[] weekday, int[] time_id)
        {
            if ( !User.IsInRole("社联") && RoleHelper.GetRoleClub(User.Identity.Name, "会长").Count == 0 )
            {
                return View("PermissionDeniedError");
            }
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
            if ( !User.IsInRole("社联") && RoleHelper.GetRoleClub(User.Identity.Name, "会长").Count == 0 )
            {
                return View("PermissionDeniedError");
            }
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
            if ( !User.IsInRole("社联") && RoleHelper.GetRoleClub(User.Identity.Name, "会长").Count == 0 )
            {
                return View("PermissionDeniedError");
            }
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
                    foreach(var item in location.UnAvailableTimes.ToList())
                    {
                        bool del_mark = true;
                        for (int i = 0; i < time_id.Length; ++i)
                        {
                            if (item.TimeId == time_id[i] && item.WeekDayId == weekday[i])
                            {
                                del_mark = false;
                            }
                        }
                        if (del_mark == true)
                        {
                            unitOfWork.LocationUnAvailableTimes.Delete(item);
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
        //// GET: /Location/Delete/5
 
        //public ActionResult Delete(int id)
        //{
        //    Location location = unitOfWork.Locations.Find(id);
        //    return View(location);
        //}

        //
        // POST: /Location/Delete/5

        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            if ( !User.IsInRole("社联") && RoleHelper.GetRoleClub(User.Identity.Name, "会长").Count == 0 )
            {
                return View("PermissionDeniedError");
            }
            Location location = unitOfWork.Locations.Find(id);
            unitOfWork.Locations.Delete(location);
            unitOfWork.SaveChanges(); 
            return Json(new { idToDelete = id, success = true });
        }

        public ActionResult Calendar()
        {
            if (!User.IsInRole("社联") && RoleHelper.GetRoleClub(User.Identity.Name, "会长").Count == 0)
            {
                return View("PermissionDeniedError");
            }
            ViewBag.Times = new SelectList(unitOfWork.Times.ToList(), "Id", "TimeName");
            return View();
        }


        public ActionResult AvailableLocation(DateTime date, int[] time_ids)
        {
            if (!User.IsInRole("社联") && RoleHelper.GetRoleClub(User.Identity.Name, "会长").Count == 0)
            {
                return View("PermissionDeniedError");
            }
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


            List<Location> locations = unitOfWork.Locations.ToList().ToList();

            foreach (var time_id in time_ids)
            {
                var assigned_locations = unitOfWork.LocationAssignments.ToList().Where(s => s.Date == date && s.Times.Any( t => t.Id == time_id));
                //删除被占用的场地
                foreach (var assigned_location in assigned_locations)
                {
                    foreach (var location in assigned_location.Locations)
                    {
                        if (locations.Contains(location))
                        {
                            locations.Remove(location);
                        }
                    }
                }


                //删除当天当时间段不可用的场地
                foreach (var location_unavailable_time in unitOfWork.LocationUnAvailableTimes.ToList())
                {
                    if (location_unavailable_time.TimeId == time_id && location_unavailable_time.WeekDayId == weekday)
                    {
                        if(locations.Contains(location_unavailable_time.Location))
                       {
                          locations.Remove(location_unavailable_time.Location);
                        }
                    }

                }

                //foreach (var location in locations)
                //{
                //    foreach (var unavailabletime in location.UnAvailableTimes)
                //    {
                //        if (unavailabletime.TimeId == time_id && unavailabletime.WeekDayId == weekday)
                //        {
                //            locations.Remove(location);
                //        }
                //    }
                //}
            }

            IEnumerable<Location> available_locations = locations.OrderBy(s => s.Name);

            ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "MajorInfo.Name");
            ViewBag.SubEventId = new SelectList(unitOfWork.SubEvents.ToList(), "Id", "Title");
            ViewBag.Date = date.ToString("yyyy年MM月dd日");
            List<Time> times = new List<Time>();
            foreach (var time_id in time_ids)
            {
                times.Add(unitOfWork.Times.Find(time_id));
            }
            ViewBag.Times = times;
            ViewBag.time_ids = time_ids;

            return View(available_locations.ToPagedList(1,10));
        }

        public ActionResult UnAvailableLocation(DateTime date, int[] time_ids)
        {
            if ( !User.IsInRole("社联") && RoleHelper.GetRoleClub(User.Identity.Name, "会长").Count == 0 )
            {
                return View("PermissionDeniedError");
            }
            int weekday = 0;
            switch (date.DayOfWeek.ToString().ToLower())
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
            List<Location> locations = new List<Location>();
            foreach (var time_id in time_ids)
            {
                //当天当时间段不可用的场地
                foreach (var location_unavailable_time in unitOfWork.LocationUnAvailableTimes.ToList())
                {
                    if (location_unavailable_time.TimeId == time_id && location_unavailable_time.WeekDayId == weekday)
                    {
                        if (!locations.Contains(location_unavailable_time.Location))
                        {
                            locations.Add(location_unavailable_time.Location);
                        }
                    }

                }
            }

            IEnumerable<Location> list = locations.OrderBy(s => s.Name);
            List<Time> times = new List<Time>();
            foreach (var time_id in time_ids)
            {
                var t = unitOfWork.Times.Find(time_id);
                times.Add(t);
            }
            ViewBag.Date = date.ToString("yyyy年MM月dd日");
            ViewBag.Times = times;
            ViewBag.time_ids = time_ids;
            return View(list.ToPagedList(1, 10));
        }

        public ActionResult AssignedLocation(DateTime date, int[] time_ids)
        {
            if (!User.IsInRole("社联") && RoleHelper.GetRoleClub(User.Identity.Name, "会长").Count == 0)
            {
                return View("PermissionDeniedError");
            }
            List<AssignedLocationViewModel> assignedLocation = new List<AssignedLocationViewModel>();
            foreach (var time_id in time_ids)
            {
                var assignments = unitOfWork.LocationAssignments.ToList().Where(s => s.Date == date && s.Times.Any(t => t.Id == time_id));
                //List<Location> locations = new List<Location>();
                foreach (var location_assignment in assignments)
                {
                    foreach (var location in location_assignment.Locations)
                    {
                        assignedLocation.Add(new AssignedLocationViewModel
                        {
                            Location = location,
                            Club = location_assignment.Club,
                            Applicant = location_assignment.Applicant
                        });
                    }
                }
            }

            IEnumerable<AssignedLocationViewModel> list = assignedLocation.OrderBy(s => s.Location.Name);

            ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "MajorInfo.Name");
            ViewBag.SubEventId = new SelectList(unitOfWork.SubEvents.ToList(), "Id", "Title");
            ViewBag.Date = date.ToString("yyyy年MM月dd日");
            List<Time> times = new List<Time>();
            foreach (var time_id in time_ids)
            {
                times.Add(unitOfWork.Times.Find(time_id));
            }
            ViewBag.Times = times;
            ViewBag.time_ids = time_ids;

            return View(list.ToPagedList(1,10));
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}