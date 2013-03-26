using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SCUTClubManager.DAL
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private SCUTClubContext mContext;
        private DbSet<T> mTable;

        public Repository()
        {
            mContext = new SCUTClubContext();
            mTable = mContext.Set<T>();
        }

        public Repository(SCUTClubContext context)
        {
            mContext = context;
            mTable = mContext.Set<T>();
        }

        public IEnumerable<T> ToList()
        {
            return mTable;
        }

        public void Delete(T instance)
        {
            if (mContext.Entry(instance).State == System.Data.EntityState.Detached)
            {
                mTable.Attach(instance);
            }

            mTable.Remove(instance);
        }

        public void Add(T instance)
        {
            mTable.Add(instance);
        }

        public void Update(T instance)
        {
            mTable.Attach(instance);
            mContext.Entry(instance).State = System.Data.EntityState.Modified;
        }

        public T Find(int id)
        {
            return mTable.Find(id);
        }
    }
}