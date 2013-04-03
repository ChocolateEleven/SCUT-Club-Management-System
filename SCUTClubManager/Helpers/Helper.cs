using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCUTClubManager.Models;

namespace SCUTClubManager.Helpers
{
    public static class HtmlHelperExtensions
    {

        public static string GetFullName(User user)
        {
            if (!(user is Student))
            {
                return "社联";
            }
            else
            {
                return (user as Student).FullName;
            }
        }

        public static string getStatus(DateTime openTime, DateTime closeTime)
        {
            if (openTime.CompareTo(DateTime.Now) > 0)
            {
                return "暂未开始";
            }
            else if (openTime.CompareTo(DateTime.Now) <= 0 &&
                closeTime.CompareTo(DateTime.Now) >= 0)
            {
                return "进行中";
            }
            else if (closeTime.CompareTo(DateTime.Now) < 0)
            {
                return "已结束";
            }

            return "";
        }
    }
}