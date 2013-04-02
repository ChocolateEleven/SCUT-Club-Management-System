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

    }
}