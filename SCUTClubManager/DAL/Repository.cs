using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Linq.Expressions;

namespace SCUTClubManager.DAL
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private SCUTClubContext context;
        private DbSet<T> table;

        public Repository()
        {
            context = new SCUTClubContext();
            table = context.Set<T>();
        }

        public Repository(SCUTClubContext context)
        {
            this.context = context;
            table = context.Set<T>();
        }

        public IQueryable<T> ToList()
        {
            return table;
        }

        public void Delete(T instance)
        {
            if (context.Entry(instance).State == System.Data.EntityState.Detached)
            {
                table.Attach(instance);
            }

            table.Remove(instance);
        }

        public void Add(T instance)
        {
            table.Add(instance);
        }

        public void Update(T instance)
        {
            context.Entry(instance).State = System.Data.EntityState.Modified;
        }

        public T Find(object id)
        {
            return table.Find(id);
        }

        public IRepository<T> Include<TProperty>(Expression<Func<T, TProperty>> path)
        {
            table.Include(path);
            return this;
        }
    }
}