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
    }
}