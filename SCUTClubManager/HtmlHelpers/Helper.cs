﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCUTClubManager.Models;

namespace SCUTClubManager.HtmlHelpers
{
    public static class HtmlHelperExtensions
    {

        public static string GetFullName(User user)
        {
            if (user is Student)
            {
                return (user as Student).FullName;
            }
            else
            {
                return "社联";
            }
        }

    }
}