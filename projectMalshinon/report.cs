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
        private report() { }

        DAL dal_Instance = DAL.GetInstance();
        public static report GetInstance()
        {
            if (_report == null)
            {
                _report = new report();
            }
            return _report;
        }

        public void start()
        {
            dal_Instance.Login();
            menu();
            //Console.WriteLine("enter your name: ");
            //string[] report_name = Console.ReadLine().Split();
            //if(!dal.People_in_Table(report_name[0], report_name[1]))
            //{
            //    dal.addPeople(report_name[0], report_name[1]);
            //}
            //Console.WriteLine("enter your report: ");
            //string report_text = Console.ReadLine();
            //string[] target_name = dal.text_report_return_name(report_text);
            //if (!dal.People_in_Table(target_name[0], target_name[1]))
            //{
            //    dal.addPeopleTarget(target_name[0], target_name[1]);
            //}
            //dal.text_report(report_text, dal.find_id(report_name[0]), dal.find_id(target_name[0]));
        }
        private int ChoiceMenu()
        {
            int choice;
            string temp;

            Console.Write("Enter your choice: ");
            temp = Console.ReadLine();

            if (int.TryParse(temp, out choice))
            {
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("The value received is incorrect.");
                ChoiceMenu();
            }
            return choice;
        }
        private void menu()
        {
            PrintMenu();
            int choice = ChoiceMenu();
            switch (choice)
            {
                case 1:
                    //דיווח
                    Console.Clear();
                    Console.WriteLine("Enter your report: ");
                    string msg = Console.ReadLine();
                    dal_Instance.InsertIntelReport(msg);
                    break;
                case 2:
                    // הצגת כל ההתראות
                    Console.Clear();
                    dal_Instance.GetAlerts();
                    break;
                case 0:
                    //יציאה מהתוכנית
                    Console.WriteLine("good day !");
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("choice: 1 or 2 or 0.");
                    break;
            }
            menu();
        }
        private void PrintMenu()
        {
            //int id = dal_Instance.target.id;
            //Console.WriteLine(id);
            Console.WriteLine("===================================");
            Console.WriteLine("||   1. Report                   ||");
            Console.WriteLine("||   2. Show all alerts          ||");
            Console.WriteLine("||   0. Exit                     ||");
            Console.WriteLine("===================================");
        }
    }
}
