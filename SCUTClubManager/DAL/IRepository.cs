using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;

namespace SCUTClubManager.DAL
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> ToList();

        void Delete(T instance);

        void Add(T instance);

        void Update(T instance);

        T Find(object id);

        IRepository<T> Include<TProperty>(Expression<Func<T, TProperty>> path);
    }
}