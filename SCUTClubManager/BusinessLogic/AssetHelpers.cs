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

        public void RemoveApplicatedAssetFrom(ICollection<ApplicatedAsset> collection, ApplicatedAsset asset_to_remove)
        {
            collection.Remove(asset_to_remove);
            db.ApplicatedAssets.Delete(asset_to_remove);
        }

        public void UpdateApplicatedAssets(ICollection<ApplicatedAsset> orig_collection, ICollection<ApplicatedAsset> new_collection)
        {
            var items_to_delete = orig_collection.Where(t => new_collection.All(s => s.Id != t.Id)).ToList();
            var items_to_add = new_collection.Where(t => orig_collection.All(s => s.Id != t.Id)).ToList();

            foreach (var item in items_to_delete)
            {
                RemoveApplicatedAssetFrom(orig_collection, item);
            }

            foreach (var item in items_to_add)
            {
                if (item.Id == 0)
                    item.Id = db.GenerateIdFor(IdentityForTPC.ASSET_BASE);

                orig_collection.Add(item);
            }
        }

        public AssetApplication JoinApplicatedAssets(AssetApplication application)
        {
            IEnumerable<ApplicatedAsset> orig_applicated_assets = application.ApplicatedAssets.ToList();
            IList<ApplicatedAsset> joined_applicated_assets = new List<ApplicatedAsset>();

            foreach (ApplicatedAsset orig_asset in orig_applicated_assets)
            {
                ApplicatedAsset joined_asset = joined_applicated_assets.FirstOrDefault(t => t.AssetId == orig_asset.AssetId);

                if (joined_asset == null)
                {
                    if (orig_asset.Id == 0)
                        orig_asset.Id = db.GenerateIdFor(IdentityForTPC.ASSET_BASE);

                    joined_applicated_assets.Add(orig_asset);
                }
                else
                {
                    joined_asset.Count += orig_asset.Count;
                }
            }

            application.ApplicatedAssets = joined_applicated_assets;

            return application;
        }

        public IEnumerable<Asset> GetAvailableAssets(DateTime date, TimeSpan start_time, TimeSpan end_time)
        {
            IEnumerable<Time> times = db.Times.ToList().ToList();
            times = times.Where(t => t.IsCoveredBy(start_time, end_time));

            return GetAvailableAssets(date, times);
        }

        public IEnumerable<Asset> GetAvailableAssets(DateTime date, IEnumerable<Time> times)
        {
            IEnumerable<Asset> assets = db.Assets.ToList();

            foreach (var time in times)
            {
                assets = GetAvailableAssets(date, time, assets);
            }

            return assets;
        }

        public IEnumerable<Asset> GetAvailableAssets(DateTime date, Time time, IEnumerable<Asset> collection = null)
        {
            IEnumerable<Asset> assets = null;

            if (collection != null)
                assets = collection;
            else
                assets = db.Assets.ToList();

            var assigned_assets = db.AssetAssignments.ToList().Where(t => t.Date == date && t.Times.Any(s => s.Id == time.Id)).Select(t => t.AssignedAssets);

            foreach (var assigned_asset_set in assigned_assets)
            {
                foreach (var assigned_asset in assigned_asset_set)
                {
                    Asset asset = assets.SingleOrDefault(t => t.Id == assigned_asset.AssetId);

                    if (asset != null)
                    {
                        asset.Count -= assigned_asset.Count;
                    }
                }
            }

            assets = assets.Where(t => t.Count > 0);

            return assets;
        }
    }
}