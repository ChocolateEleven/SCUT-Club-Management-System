using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCUTClubManager.Models;
using SCUTClubManager.DAL;
using SCUTClubManager.Helpers;

namespace SCUTClubManager.BusinessLogic
{
    public class EventHelpers
    {
        private UnitOfWork db = null;

        public EventHelpers(UnitOfWork context)
        {
            db = context;
        }

        public void DeleteEvent(Event e, bool save = true)
        {
            // 删除与该活动相关的资金申请
            if (e.FundApplication != null)
            {
                db.FundApplications.Delete(e.FundApplication);
            }

            // 删除该活动的海报
            if (!String.IsNullOrWhiteSpace(e.PosterUrl))
            {
                HtmlHelpersExtensions.DeleteFileFrom(ConfigurationManager.EventPosterFolder, e.PosterUrl);
            }

            // 删除该活动的策划书
            if (!String.IsNullOrWhiteSpace(e.PlanUrl))
            {
                HtmlHelpersExtensions.DeleteFileFrom(ConfigurationManager.EventPlanFolder, e.PlanUrl);
            }

            // 删除该活动的所有子活动
            if (e.SubEvents != null)
            {
                foreach (var sub_event in e.SubEvents)
                {
                    DeleteSubEvent(sub_event, e.SubEvents, true);
                }
            }

            db.Events.Delete(e);

            if (save)
            {
                db.SaveChanges();
            }
        }

        public void NewLocationApplication(LocationApplication application, SubEvent sub_event, bool no_add = true, bool save = false)
        {
            application.Id = db.GenerateIdFor(IdentityForTPC.APPLICATION);
            application.ApplicantUserName = sub_event.Event.ChiefEventOrganizerId;
            application.ApplicatedDate = sub_event.Date;
            application.ClubId = sub_event.Event.ClubId;
            application.Date = DateTime.Now;
            application.Status = Application.NOT_SUBMITTED;
            application.SubEvent = sub_event;
            application.Times = sub_event.Times;

            ICollection<Location> collection = application.Locations.ToList();
            var locations = db.Locations.ToList().ToList().Where(t => collection.Any(s => s.Id == t.Id));

            application.Locations.Clear();
            foreach (Location location in locations)
            {
                application.Locations.Add(location);
            }

            if (!no_add)
                sub_event.LocationApplications.Add(application);

            if (save)
                db.SaveChanges();
        }

        public void NewAssetApplication(AssetApplication application, SubEvent sub_event, bool no_add = true, bool save = false)
        {
            application.Id = db.GenerateIdFor(IdentityForTPC.APPLICATION);
            application.ApplicantUserName = sub_event.Event.ChiefEventOrganizerId;
            application.ApplicatedDate = sub_event.Date;
            application.ClubId = sub_event.Event.ClubId;
            application.Date = DateTime.Now;
            application.Status = Application.NOT_SUBMITTED;
            application.SubEvent = sub_event;
            application.Times = sub_event.Times;

            foreach (ApplicatedAsset asset in application.ApplicatedAssets)
            {
                asset.Id = db.GenerateIdFor(IdentityForTPC.ASSET_BASE);
                asset.AssetApplicationId = application.Id;
            }

            if (!no_add)
                sub_event.AssetApplications.Add(application);

            if (save)
                db.SaveChanges();
        }

        public void UpdateLocationApplication(LocationApplication orig_application, LocationApplication new_application)
        {
            orig_application.ApplicatedDate = orig_application.SubEvent.Date;
            orig_application.Date = DateTime.Now;

            UpdateTimes(orig_application.Times, orig_application.SubEvent.Times);

            orig_application.Locations.Clear();

            ICollection<Location> collection = new_application.Locations.ToList();
            var locations = db.Locations.ToList().ToList().Where(t => collection.Any(s => s.Id == t.Id));

            foreach (Location location in locations)
            {
                orig_application.Locations.Add(location);
            }
        }

        public void UpdateAssetApplication(AssetApplication orig_application, AssetApplication new_application)
        {
            orig_application.ApplicatedDate = orig_application.SubEvent.Date;
            orig_application.Date = DateTime.Now;

            UpdateTimes(orig_application.Times, orig_application.SubEvent.Times);

            AssetHelpers helper = new AssetHelpers(db);
            helper.UpdateApplicatedAssets(orig_application.ApplicatedAssets, new_application.ApplicatedAssets);
            //foreach (ApplicatedAsset asset in new_application.ApplicatedAssets)
            //{
            //    if (asset.Id == 0)
            //        asset.Id = db.GenerateIdFor(IdentityForTPC.ASSET_BASE);

            //    orig_application.ApplicatedAssets.Add(asset);
            //    asset.AssetApplicationId = orig_application.Id;
            //}
        }

        public void UpdateTimes(ICollection<Time> orig_collection, ICollection<Time> new_collection)
        {
            var times_to_delete = orig_collection.Where(t => new_collection.All(s => s.Id != t.Id)).ToList();
            var times_to_add = new_collection.Where(t => orig_collection.All(s => s.Id != t.Id)).ToList();

            foreach (var time in times_to_delete)
            {
                orig_collection.Remove(time);
            }

            foreach (var time in times_to_add)
            {
                orig_collection.Add(time);
            }
        }
        
        public void DeleteSubEvent(SubEvent sub_event, ICollection<SubEvent> from = null, bool no_delete = false, bool save = false)
        {
            // 删除与该子活动相关的场地申请
            if (sub_event.LocationApplications != null)
            {
                var locations_applications = sub_event.LocationApplications.ToList();

                foreach (var location_app in locations_applications)
                {
                    db.LocationApplications.Delete(location_app);
                }
            }

            // 删除与该子活动相关的物资申请
            if (sub_event.AssetApplications != null)
            {
                var asset_applications = sub_event.AssetApplications.ToList();

                foreach (var asset_app in asset_applications)
                {
                    db.AssetApplications.Delete(asset_app);
                }
            }

            // 删除该子活动本身
            if (!no_delete)
            {
                if (from == null)
                {
                    db.SubEvents.Delete(sub_event);
                }
                else
                {
                    from.Remove(sub_event);
                }
            }

            if (save)
            {
                db.SaveChanges();
            }
        }
    }
}