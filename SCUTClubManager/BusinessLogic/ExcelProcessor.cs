using System;
using System.Collections.Generic;
using LinqToExcel;
using System.Linq;
using System.Web;
using System.IO;
using SCUTClubManager.Models;

namespace SCUTClubManager.BusinessLogic
{
    public class ExcelProcessor : IDisposable
    {
        private ExcelQueryFactory excel;
        private bool hasInit = false;
        private bool disposed = false;

        private int rowsPerRun;
        public int RowsPerRun
        {
            get
            {
                return rowsPerRun;
            }
        }

        private string fileName;
        public string FileName
        {
            get
            {
                return fileName;
            }
        }

        private int rowsRead;
        public int RowsRead
        {
            get
            {
                return rowsRead;
            }
        }

        private int totalRows;
        public int TotalRows
        {
            get
            {
                return totalRows;
            }
        }

        public ExcelProcessor(int rows_per_run, string file_name)
        {
            rowsPerRun = rows_per_run;
            fileName = file_name;
            rowsRead = 0;
            totalRows = 0;
        }

        public void Init()
        {
            if (!hasInit)
            {
                hasInit = true;

                excel = new ExcelQueryFactory(fileName);
                excel.DatabaseEngine = LinqToExcel.Domain.DatabaseEngine.Ace;

                totalRows = (from r in excel.WorksheetNoHeader()
                        select r).Count();
            }
        }

        public IEnumerable<User> FetchMoreUsers()
        {
            List<User> users = new List<User>(rowsPerRun);
            int admin_role_id = ScmRoleProvider.GetRoleIdByName("社联");
            int index_to_read_to = (rowsRead + rowsPerRun) > totalRows ? totalRows : (rowsRead + rowsPerRun);
            string start = "A" + (rowsRead + 1);
            string end = "D" + index_to_read_to;

            // 已经读完
            if (totalRows <= rowsRead)
                return users;

            users.AddRange(from r in excel.WorksheetRange<User>(start, end)
                           where r.RoleId == admin_role_id
                           select r);
            users.AddRange(from r in excel.WorksheetRange<Student>(start, end)
                           where r.RoleId != admin_role_id
                           select r);

            return users;
        }

        public bool IsEnd()
        {
            return rowsRead >= totalRows;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}