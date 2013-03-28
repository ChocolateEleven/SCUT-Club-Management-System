using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.Data.Entity;
using PagedList;

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
        public static IEnumerable<T> Query<T>(IEnumerable<T> collection, Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> order_by = null, string[] includes = null, int? page_number = null, int? items_per_page = null)
            where T : class
        {
            IPagedList<T> paged_list = null;

            if (collection != null && collection is DbSet<T>)
            {
                DbSet<T> db_set = collection as DbSet<T>;
                IQueryable<T> query = db_set as IQueryable<T>;

                // 包含。
                if (includes != null)
                {
                    foreach (var include in includes)
                    {
                        query.Include(include);
                    }
                }

                if (filter != null)
                {
                    // 过滤（搜索）。

                        query = query.Where(filter);

                }

                // 排序。
                if (order_by != null)
                {
                    query = order_by(query);
                }

                // 分页。
                if (page_number.HasValue && items_per_page.HasValue)
                {
                    paged_list = query.ToPagedList(page_number.Value, items_per_page.Value);
                }
                else
                {
                    return query;
                }
            }

            return paged_list;
        }
    }
}