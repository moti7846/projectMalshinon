using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace projectMalshinon
{
    internal class report
    {
        static report _report = null;


        private string connStr = "server=localhost;user=root;password=;database=malshinon";
        private MySqlConnection _conn;
        private report() { }

        public static report GetInstance()
        {
            if (_report == null)
            {
                _report = new report();
            }
            return _report;
        }

        public void searchThePeopleTable(string first_name, string last_name)
        {
            try
            {
                using (var connection = new MySqlConnection(connStr))
                {
                    connection.Open();
                    string q = $"SELECT * FROM `people` WHERE first_name = '{first_name}'";
                    var cmd = new MySqlCommand(q, connection);
                    var reader = cmd.ExecuteReader();
                    if(!reader.Read())
                    {
                        addPeople(first_name, last_name);
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("MySQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error: " + ex.Message);
            }
        }

        public void addPeople(string first_name, string last_name)
        {
            try
            {
                using (var connection = new MySqlConnection(connStr))
                {
                    connection.Open();
                    Guid g = Guid.NewGuid();
                    string t = Convert.ToBase64String(g.ToByteArray()).Substring(2, 8);
                    string q = $"INSERT INTO `people` (first_name, last_name, type_people, secret_code) VALUES('{first_name}', '{last_name}', 'reporter', '{t}')";
                    var cmd = new MySqlCommand(q, connection);
                    cmd.ExecuteReader();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("MySQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error: " + ex.Message);
            }
        }
         
        public void find_id(string Fname)
        {
            connection.Open();
            string q = $"SELECT * FROM `people` WHERE first_name = '{first_name}'";
            var cmd = new MySqlCommand(q, connection);
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                addPeople(first_name, last_name);
            }
        }

        public void text_report(string str)
        {
            string Fname = null;
            string lname = null;

            string[] parts = str.Split(' ');
            foreach (string s in parts)
            {
                if (char.IsUpper(s[0]))
                {
                    if (Fname == null)
                    {
                        Fname = s;
                    }
                    else if (lname == null)
                    {
                        lname = s;
                        break;
                    }
                }
            }
            searchThePeopleTable(Fname, lname);

        }
    }
}
