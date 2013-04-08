using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.Data.Entity;
using PagedList;
using SCUTClubManager.Helpers;
using SCUTClubManager.Models;

namespace SCUTClubManager.BusinessLogic
{
    /**
     *  用于对集合进行过滤（搜索）、排序、包含、分页等操作的帮助类。
     */
    public static class QueryProcessor
    {
        /**
         *  对给定集合进行过滤（搜索）、排序、包含、分页等操作中的一项或多项。一个参数为空则代表不对该集合进行相关的操作。
         *  @param collection 要对其进行操作的集合。
         *  @param filters 一组过滤器。
         *  @param order_by 排序条件。
         *  @param includes 要包含的数据项。
         *  @param page_number 要返回的页码。
         *  @param items_per_page 每页包含的项目数。
         *  @returns 完成做操后的集合。
         */
        public static IPagedList<T> Query<T>(IEnumerable<T> collection, Expression<Func<T, bool>> filter = null, 
            String order_by = null, string[] includes = null, int? page_number = null, int? items_per_page = null)
            where T : class
        {
            IPagedList<T> paged_list = null;

            if (collection != null && collection is IQueryable<T>)
            {
                IQueryable<T> db_set = collection as IQueryable<T>;
                IEnumerable<T> query = null;
                              
                // 包含。
                if (includes != null)
                {
                    foreach (var include in includes)
                    {
                        db_set = db_set.Include(include);
                    }
                }

                // 过滤（搜索）。
                if (filter != null)
                {
                    db_set = db_set.Where(filter);
                }

                query = db_set;

                // 排序。
                if (order_by != null)
                {
                    if (order_by.EndsWith("Desc") || order_by.EndsWith("Descending"))
                    {
                        query = query.OrderByDescending(order_by.Substring(0, order_by.IndexOf("Desc")));
                    }
                    else
                    {
                        query = query.OrderBy(order_by);
                    }
                }

                // 分页。

                // 处理坑爹情况1
                if (page_number < 1)
                {
                    page_number = 1;
                }

                if (page_number.HasValue && items_per_page.HasValue)
                {
                    paged_list = query.ToPagedList(page_number.Value, items_per_page.Value);

                    if (paged_list.PageCount != 0)
                    {
                        // 坑爹2
                        if (page_number > paged_list.PageCount)
                        {
                            paged_list = query.ToPagedList(paged_list.PageCount, items_per_page.Value);
                        }
                    }
                }
                else
                {
                    paged_list = query.ToPagedList(1, query.Count());
                }
            }

            return paged_list;
        }

        public static IQueryable<Application> FilterApplication(IQueryable<Application> collection, string pass_filter = "", 
            string type_filter = "", int club_id = 0)
        {
            var applications = collection;

            switch (pass_filter)
            {
                case "Passed":
                    applications = applications.Where(s => s.Status == "p");
                    break;

                case "Failed":
                    applications = applications.Where(s => s.Status == "f");
                    break;

                case "NotVerified":
                    applications = applications.Where(s => s.Status == "n");
                    break;

                case "Verified":
                    applications = applications.Where(s => s.Status == "p" || s.Status == "f");
                    break;

                default:
                    break;
            }

            switch (type_filter)
            {
                case "Register":
                    applications = applications.Where(t => t is ClubRegisterApplication);
                    break;

                case "Unregister":
                    applications = applications.Where(t => t is ClubUnregisterApplication);
                    break;

                case "InfoModify":
                    applications = applications.Where(t => t is ClubInfoModificationApplication);
                    break;

                case "ClubTransaction":
                    applications = applications.Where(t => t is ClubRegisterApplication || t is ClubUnregisterApplication || t is ClubInfoModificationApplication);
                    break;

                case "Location":
                    applications = applications.Where(t => t is LocationApplication);
                    break;

                case "Asset":
                    applications = applications.Where(t => t is AssetApplication);
                    break;

                case "Fund":
                    applications = applications.Where(t => t is FundApplication);
                    break;

                case "ClubMember":
                    applications = applications.Where(t => t is ClubApplication);
                    break;

                default:
                    break;
            }

            if (club_id != 0)
            {
                applications = applications.Where(t => t.ClubId == club_id);
            }

            return applications;
        }
    }
}