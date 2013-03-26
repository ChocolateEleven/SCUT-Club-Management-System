using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.DAL
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> ToList();
        void Delete(T instance);
        void Add(T instance);
        void Update(T instance);
    }
}