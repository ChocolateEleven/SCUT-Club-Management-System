using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCUTClubManager.Models;
using SCUTClubManager.DAL;
using SCUTClubManager.Helpers;

namespace SCUTClubManager.BusinessLogic
{
    public class LocationHelpers
    {
        private UnitOfWork db = null;

        public LocationHelpers(UnitOfWork context)
        {
            db = context;
        }

        public void VerifyLocationApplication(LocationApplication application, bool is_passed, bool save)
        {
            if (is_passed)
            {
                application.Status = Application.PASSED;

                LocationAssignment assignment = new LocationAssignment
                {
                    ApplicantUserName = application.ApplicantUserName,
                    ClubId = application.ClubId ?? 0,
                    Date = application.ApplicatedDate,
                    Locations = application.Locations,
                    Times = application.Times
                };

                application.Assignment = assignment;
            }
            else
            {
                application.Status = Application.FAILED;
            }

            if (save)
                db.SaveChanges();
        }

        public IEnumerable<Location> GetAvailableLocations(DateTime date, TimeSpan start_time, TimeSpan end_time)
        {
            IEnumerable<Time> times = db.Times.ToList().ToList();
            times = times.Where(t => t.IsCoveredBy(start_time, end_time));

            return GetAvailableLocations(date, times);
        }

        public IEnumerable<Location> GetAvailableLocations(DateTime date, IEnumerable<Time> times)
        {
            IEnumerable<Location> locations = db.Locations.ToList();

            foreach (var time in times)
            {
                locations = GetAvailableLocations(date, time, locations);
            }

            return locations;
        }

        public IEnumerable<Location> GetAvailableLocations(DateTime date, Time time, IEnumerable<Location> collection = null)
        {
            string weekday_name = date.DayOfWeek.ToString();
            int weekday_id = GetWeekDayIdByName(weekday_name);

            IEnumerable<Location> locations = null;

            if (collection != null)
                locations = collection;
            else
                locations = db.Locations.ToList();

            // 过滤掉被占用的场地
            var assigned_locations = db.LocationAssignments.ToList().Where(s => s.Date == date && s.Times.Any(t => t.Id == time.Id)).Select(t => t.Locations);
            locations = locations.Where(t => assigned_locations.All(s => s.All(f => f.Id != t.Id)));

            // 过滤掉当天当时间段不可用的场地
            locations = locations.Where(t => t.UnAvailableTimes.All(s => s.WeekDayId != weekday_id || s.TimeId != time.Id));

            return locations;
        }

        public LocationApplication JoinLocations(LocationApplication application)
        {
            IDictionary<int, Location> location_occurance_counter = new Dictionary<int, Location>();
            List<Location> joined_locations = new List<Location>();

            foreach (Location location in application.Locations)
            {
                if (!location_occurance_counter.ContainsKey(location.Id))
                    location_occurance_counter.Add(new KeyValuePair<int, Location>(location.Id, null));

                location_occurance_counter[location.Id] = location;
            }

            foreach (var location in location_occurance_counter)
            {
                joined_locations.Add(location.Value);
            }

            application.Locations = joined_locations;

            return application;
        }

        private int GetWeekDayIdByName(string name)
        {
            int weekday = 0;

            switch (name.ToLower())
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

            return weekday;
        }
    }
}