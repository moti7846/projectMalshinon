using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectMalshinon
{
    internal class Program
    {
        public static void test(string str)
        {
            string Fname = null;
            string lname = null;

            string[] parts = str.Split(' ');
            foreach (string s in parts)
            {
                if (char.IsUpper(s[0]))
                {
                    if(Fname == null)
                    {
                        Fname = s;
                    }
                    else if(lname == null)
                    {
                        lname = s;
                        break;
                    }
                }
            }
            Console.WriteLine(Fname);
            Console.WriteLine(lname);
            Console.WriteLine(str);

        }


        static void Main(string[] args)
        {
            //report a = report.GetInstance();
            //a.searchThePeopleTable("moti", "moiz");
            //a.searchThePeopleTable("moti", "moiz");
            //a.searchThePeopleTable("aaaa", "moiz");
            //a.searchThePeopleTable("moti", "moz");
            test("my name is Moti Minzberg i liked piza");

        }
    }
}
