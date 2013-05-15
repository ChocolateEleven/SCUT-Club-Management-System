using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCUTClubManager.Models;
using SCUTClubManager.DAL;
using SCUTClubManager.Helpers;

namespace SCUTClubManager.BusinessLogic
{
    public class TimeHelpers
    {
        private UnitOfWork db = null;

        public TimeHelpers(UnitOfWork context)
        {
            db = context;
        }

        public ICollection<Time> JoinTimes(ICollection<Time> collection)
        {
            IDictionary<int, Time> time_occurance_counter = new Dictionary<int, Time>();
            List<Time> joined_times = new List<Time>();

            foreach (Time time in collection)
            {
                if (!time_occurance_counter.ContainsKey(time.Id))
                    time_occurance_counter.Add(new KeyValuePair<int, Time>(time.Id, null));

                time_occurance_counter[time.Id] = time;
            }

            foreach (var time in time_occurance_counter)
            {
                joined_times.Add(time.Value);
            }

            return joined_times;
        }
    }
}