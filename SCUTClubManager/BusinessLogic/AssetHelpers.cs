using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCUTClubManager.Models;
using SCUTClubManager.DAL;
using SCUTClubManager.Helpers;

namespace SCUTClubManager.BusinessLogic
{
    public class AssetHelpers
    {
        private UnitOfWork db = null;

        public AssetHelpers(UnitOfWork context)
        {
            db = context;
        }

        public void VerifyAssetApplication(AssetApplication application, bool is_passed, bool save = true)
        {
            if (is_passed)
            {
                application.Status = Application.PASSED;

                AssetAssignment assignment = new AssetAssignment
                {
                    ApplicantUserName = application.ApplicantUserName,
                    ClubId = application.ClubId ?? 0,
                    Date = application.ApplicatedDate,
                    Times = application.Times,
                    AssignedAssets = new List<AssignedAsset>()
                };

                foreach (ApplicatedAsset asset in application.ApplicatedAssets)
                {
                    assignment.AssignedAssets.Add(new AssignedAsset
                    {
                        Id = db.GenerateIdFor(IdentityForTPC.ASSET_BASE),
                        AssetId = asset.AssetId,
                        Count = asset.Count
                    });
                }

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