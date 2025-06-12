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
                //case 3:
                //    // הצגת כל ההתראות
                //    Console.Clear();
                //    dal_Instance.GetAllPotentialAgent();
                //    break;
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
            int id = dal_Instance.report.id;
            Console.WriteLine($"Hello id: {id}");
            Console.WriteLine("===================================");
            Console.WriteLine("||   1. Report                   ||");
            Console.WriteLine("||   2. Show all alerts          ||");
            Console.WriteLine("||   3. Show all potential agent ||");
            Console.WriteLine("||   0. Exit                     ||");
            Console.WriteLine("===================================");
        }
    }
}
