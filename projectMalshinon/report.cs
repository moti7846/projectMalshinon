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
        reportDAL dal = null;
        private report() { }

        private report GetInstance()
        {
            if (_report == null)
            {
                _report = new report();
            }
            return _report;
        }

        public static void start()
        {
            reportDAL dal = reportDAL.GetInstance();
            Console.WriteLine("enter your name: ");
            string[] report_name = Console.ReadLine().Split();
            if(!dal.People_in_Table(report_name[0], report_name[1]))
            {
                dal.addPeople(report_name[0], report_name[1]);
            }
            Console.WriteLine("enter your report: ");
            string report_text = Console.ReadLine();
            string[] target_name = dal.text_report_return_name(report_text);
            if (!dal.People_in_Table(target_name[0], target_name[1]))
            {
                dal.addPeopleTarget(target_name[0], target_name[1]);
            }
            dal.text_report(report_text, dal.find_id(report_name[0]), dal.find_id(target_name[0]));

        }
    }
}
