using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCUTClubManager.Models;

namespace SCUTClubManager.Helpers
{
    public static class LabelHelpers
    {
        public static int DELETED_ID = -1;
        public static int CREATED_ID = 0;

        public static string GetStatus(DateTime openTime, DateTime closeTime)
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

        public static string GetStatus(string status_code)
        {
            status_code = status_code.ToLower();

            switch (status_code)
            {
                case Application.NOT_VERIFIED:
                    return "未审批";
                case Application.PASSED:
                    return "通过";
                case Application.FAILED:
                    return "驳回";
                default:
                    return "未知";
            }
        }

        public static HtmlString GetComparedText(object orig_obj, object new_obj)
        {
            string result = "";

            if (!orig_obj.GetType().Equals(new_obj.GetType()))
            {
                throw new ArgumentException();
            }

            if (orig_obj.Equals(new_obj))
            {
                result += "<div class=\"NoChangeComparedText\">" + orig_obj.ToString() + "</div>";
            }
            else
            {
                result += "<div class=\"ChangedComparedText\">" + new_obj.ToString() + 
                    "</div><div class=\"ChangedComparedText\">(原为" + orig_obj.ToString() + ")</div>";
            }

            return new HtmlString(result);
        }

        public static string GetGender(string gender_id)
        {
            gender_id = gender_id.ToLower();

            switch (gender_id)
            {
                case "m":
                    return "男";
                case "f":
                    return "女";
                default:
                    return "未知";
            }
        }

        public static string GetPoliticalRole(string political_id)
        {
            political_id = political_id.ToLower();

            switch (political_id)
            {
                case "q":
                    return "群众";
                case "d":
                    return "共产党员";
                case "y":
                    return "预备党员";
                case "t":
                    return "共青团员";
                case "o":
                    return "其他党派";
                default:
                    return "未知";
            }
        }

        public static string GetDegree(string degree_id)
        {
            degree_id = degree_id.ToLower();

            switch (degree_id)
            {
                case "b":
                    return "本科";
                case "m":
                    return "硕士";
                case "p":
                    return "博士";
                default:
                    return "未知";
            }
        }

        public static string GetEventStatus(Event e)
        {
            string status_code = e.Status.ToLower();

            switch (status_code)
            {
                case Application.FAILED:
                    return "被驳回";
                case Application.NOT_VERIFIED:
                    return "未审批";
                case Application.PASSED:
                    if (e.Date.Date > DateTime.Now.Date)
                        return "未开始";
                    else if (e.SubEvents.Any(t => t.Date.Date == DateTime.Now.Date))
                        return "进行中";
                    else
                        return "已结束";
                default:
                    return "未知";
            }
        }
    }
}