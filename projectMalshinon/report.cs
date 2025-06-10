using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace projectMalshinon
{
    internal class report
    {
        static report _report = null;
        reportDAL dal = reportDAL.GetInstance();
        private report() { }

        public static report GetInstance()
        {
            if (_report == null)
            {
                _report = new report();
            }
            return _report;
        }

        public void Plogging(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
